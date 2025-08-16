using System;
using System.Drawing;
using System.Globalization;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using IVec2 = PaintDotNet.Rendering.Vector2Int32;

namespace LolK.PaintDotNet.Plugins;

internal static class Constants {
  public const string EffectName = "Paint.XOR";
  public const string Description = "XOR pattern effect for Paint.NET";
  public const string Version = "1.4.0";
  public const string Author = "catiqueue";
  public const string Culture = "en-US";
  public const string Website = "https://github.com/catiqueue";
  public const string Copyright = $"Copyright © {Author} 2025";
  public const string Base64Image = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsEAAA7BAbiRa+0AAABVSURBVDhPfYpBDsAwCMP4/6fZJtSGhs4+RAQnXjJzZ0Efcgv/9N53hVvvJ2Snmyl4Vyl65/V3e+e1X7e1W++87vytewreVQpy03o/udu5pg+5hT4RD2PJHvDN2VqUAAAAAElFTkSuQmCC";
  public const int Base64ImageStringLength = 192;
}

internal delegate bool Filter(int magic);
internal delegate ManagedColor Painter(int magic);
internal delegate int Operation(IVec2 pos);
internal enum OperationChoice { XOR, AND, OR }
internal enum FilterChoice { IsPrime, IsDivisible }

internal static class Create {
  public static class Filter {
    public static Plugins.Filter IsPrime => MathHelpers.IsPrime;
    public static Plugins.Filter DivisibleBy(int divisor) => magic => MathHelpers.IsDivisible(magic, divisor);
  }
  public static class Painter {
    public static Plugins.Painter SineHsvPainter => magic => ManagedColor.Create((SrgbColorA) new ColorHsv96Float(
      hue: MathF.Sin(magic) * 180f + 180f,
      saturation: 100f,
      value: MathF.Cos(magic) * 50f + 50f).ToRgb());
    public static Plugins.Painter ConstantColorPainter(ManagedColor color) => _ => color;
  }
  public static class Operation {
    public static Plugins.Operation XOR => pos => pos.X ^ pos.Y;
    public static Plugins.Operation AND => pos => pos.X & pos.Y;
    public static Plugins.Operation OR => pos => pos.X | pos.Y;
    public static Plugins.Operation FromChoice(OperationChoice choice) => choice switch {
      OperationChoice.XOR => XOR,
      OperationChoice.AND => AND,
      OperationChoice.OR => OR,
      _ => throw new ArgumentOutOfRangeException(nameof(choice), "Unknown operation choice")
    };
  }
}

internal readonly record struct CameraParameters(IVec2 Offset, int Zoom) {
  public IVec2 ApplyTo(IVec2 pos) => pos / Zoom + Offset;
}
internal sealed class PaintDotXorSettings {
  public Operation Operation { get; set; } = Create.Operation.XOR;
  public Filter Filter { get; set; } = Create.Filter.IsPrime;
  public Painter Painter { get; set; } = Create.Painter.SineHsvPainter;
  public CameraParameters CameraParameters { get; set; } = new(new IVec2(0, 0), 1);
}

[PluginSupportInfo(typeof(PluginSupportInfo))]
public class PaintDotXor() : PropertyBasedBitmapEffect(Info.DisplayName, Info.Icon, PluginSupportInfo.SubMenu, BitmapEffectOptionsFactory.Create() with { IsConfigurable = true }) {
  private static readonly PluginSupportInfo Info = new();
  private PaintDotXorSettings Settings { get; } = new();

  private enum PropertyNames { OffsetX, OffsetY, Zoom, FilterMode, Divisor, Operation, UseHSV, Color }

  protected override void OnSetToken(PropertyBasedEffectConfigToken? newToken) {
    if(newToken == null) return;
    
    Settings.CameraParameters = new CameraParameters(
      Offset: new IVec2(
        newToken.GetProperty<Int32Property>(PropertyNames.OffsetX)!.Value, 
        newToken.GetProperty<Int32Property>(PropertyNames.OffsetY)!.Value),
      Zoom: newToken.GetProperty<Int32Property>(PropertyNames.Zoom)!.Value
    );
    
    Settings.Operation = Create.Operation.FromChoice((OperationChoice)newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.Operation)!.Value);
    
    Settings.Filter = (FilterChoice) newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.FilterMode)!.Value switch {
      FilterChoice.IsDivisible => Create.Filter.DivisibleBy(newToken.GetProperty<Int32Property>(PropertyNames.Divisor)!.Value),
      FilterChoice.IsPrime => Create.Filter.IsPrime,
      _ => throw new ArgumentOutOfRangeException(nameof(newToken), "Unknown filter choice!")
    };

    Settings.Painter = newToken.GetProperty<BooleanProperty>(PropertyNames.UseHSV)!.Value
      ? Create.Painter.SineHsvPainter
      : Create.Painter.ConstantColorPainter(newToken.GetProperty<ManagedColorProperty>(PropertyNames.Color)!.Value);
    
    base.OnSetToken(newToken);
  }

  protected override void OnRender(IBitmapEffectOutput output) {
    // should I dispose this? I think it's managed by Paint.NET
    /* using */ var source = Environment.GetSourceBitmapBgra32();
    using var sourceLock = source.Lock(source.Bounds());
    var sourceRegion = sourceLock.AsRegionPtr();
    using var outputLock = output.LockBgra32();
    var outputRegion = outputLock.AsRegionPtr();
    var pos = new IVec2();

    for (pos.Y = 0; pos.Y < outputRegion.Height && !IsCancelRequested; pos.Y++) {
      for (pos.X = 0; pos.X < outputRegion.Width; pos.X++) {
        int magic = Settings.Operation(Settings.CameraParameters.ApplyTo(pos));
        outputRegion[pos.X, pos.Y] = Settings.Filter(magic) ? Settings.Painter(magic).GetSrgb() : sourceRegion[pos.X, pos.Y];
      }
    }
  }
  
  protected override PropertyCollection OnCreatePropertyCollection() => new(
    properties: [
      new Int32Property(PropertyNames.OffsetX, 0, 0, ushort.MaxValue),
      new Int32Property(PropertyNames.OffsetY, 0, 0, ushort.MaxValue),
      new Int32Property(PropertyNames.Zoom, 1, 1, 128),
      StaticListChoiceProperty.CreateForEnum<FilterChoice>(PropertyNames.FilterMode, 0, false),
      new Int32Property(PropertyNames.Divisor, 1, 1, 512),
      StaticListChoiceProperty.CreateForEnum<OperationChoice>(PropertyNames.Operation, 0, false),
      new BooleanProperty(PropertyNames.UseHSV, true),
      new ManagedColorProperty(PropertyNames.Color, ManagedColor.Create(SrgbColors.Black))
    ], 
    rules: [
      new ReadOnlyBoundToValueRule<object,StaticListChoiceProperty>(PropertyNames.Divisor, PropertyNames.FilterMode, FilterChoice.IsPrime, false),
      new ReadOnlyBoundToBooleanRule(PropertyNames.Color, PropertyNames.UseHSV, false)
    ]);

  protected override ControlInfo OnCreateConfigUI(PropertyCollection properties) {
    ControlInfo configUi = CreateDefaultConfigUI(properties);

    configUi.SetPropertyControlValue(PropertyNames.OffsetX, ControlInfoPropertyNames.DisplayName, "X offset");
    configUi.SetPropertyControlValue(PropertyNames.OffsetY, ControlInfoPropertyNames.DisplayName, "Y offset");

    configUi.SetPropertyControlValue(PropertyNames.Zoom, ControlInfoPropertyNames.DisplayName, "Zoom");

    configUi.SetPropertyControlValue(PropertyNames.FilterMode, ControlInfoPropertyNames.DisplayName, "Filter mode");
    configUi.FindControlForPropertyName(PropertyNames.FilterMode)!.SetValueDisplayName(FilterChoice.IsPrime, "Is prime?");
    configUi.FindControlForPropertyName(PropertyNames.FilterMode)!.SetValueDisplayName(FilterChoice.IsDivisible, "Is divisible?");

    configUi.SetPropertyControlValue(PropertyNames.Divisor, ControlInfoPropertyNames.DisplayName, "Divisor");

    configUi.SetPropertyControlValue(PropertyNames.Operation, ControlInfoPropertyNames.DisplayName, "Operation");
    configUi.FindControlForPropertyName(PropertyNames.Operation)!.SetValueDisplayName(OperationChoice.XOR, "XOR");
    configUi.FindControlForPropertyName(PropertyNames.Operation)!.SetValueDisplayName(OperationChoice.AND, "AND");
    configUi.FindControlForPropertyName(PropertyNames.Operation)!.SetValueDisplayName(OperationChoice.OR, "OR");

    configUi.SetPropertyControlValue(PropertyNames.UseHSV, ControlInfoPropertyNames.DisplayName, "");
    configUi.SetPropertyControlValue(PropertyNames.UseHSV, ControlInfoPropertyNames.Description, "Use HSV conversion");

    configUi.SetPropertyControlValue(PropertyNames.Color, ControlInfoPropertyNames.DisplayName, "Pattern color");
    configUi.SetPropertyControlType(PropertyNames.Color, PropertyControlType.ColorWheel);
    configUi.SetPropertyControlValue(PropertyNames.Color, ControlInfoPropertyNames.ShowResetButton, false);

    return configUi;
  }
  
  protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection properties) {
    properties[ControlInfoPropertyNames.WindowHelpContentType]!.Value = WindowHelpContentType.PlainText;
    properties[ControlInfoPropertyNames.WindowHelpContent]!.Value = string.Format(PluginSupportInfo.Culture, "{0}\n{1} v{2}\n{3}\nAll rights reserved.", Info.Copyright, Info.DisplayName, Info.Version, Info.Author);

    base.OnCustomizeConfigUIWindowProperties(properties);
  }
}

internal static class MathHelpers {
  // That's how it was originally written in Utils.Math
  public static bool IsPrime(int num) {
    if ((num <= 1) || (num % 2 == 0 && num > 2)) return false;
    for (int i = 3; i < Math.Floor(Math.Sqrt(num)); i += 2)
      if (num % i == 0)
        return false;
    return true;
  }
  public static bool IsDivisible(int num, int divisor) => num % divisor == 0;
}

internal sealed class PluginSupportInfo : IPluginSupportInfo {
  // These should be also static, but oh well, we're implementing an interface.
  // I wonder if there's a way to set these up without the attribute.
  public string Author => Constants.Author;
  public string Copyright => Constants.Copyright;
  public string DisplayName => Constants.EffectName;
  public Version Version { get; } = new(Constants.Version);
  public Uri WebsiteUri { get; } = new(Constants.Website);
  
  public static CultureInfo Culture { get; } = new(Constants.Culture);
  // Oops. The stream is not disposed. Who cares in this case?
  public Image Icon { get; } = Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String(Constants.Base64Image), 0, Constants.Base64ImageStringLength), true);
  public static string SubMenu => SubmenuNames.Render;
}