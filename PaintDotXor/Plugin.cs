using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.PaintDotXor.Types;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXor;

[PluginSupportInfo(typeof(PluginSupportInfo))]
public sealed class Plugin() : PropertyBasedBitmapEffect(Info.DisplayName, Info.Icon, PluginSupportInfo.SubMenu, BitmapEffectOptionsFactory.Create() with { IsConfigurable = true }) {
  private static readonly PluginSupportInfo Info = new();
  private Settings Settings { get; } = new();

  private enum PropertyNames { OffsetX, OffsetY, Zoom, FilterMode, Divisor, Operation, UseHSV, Color }

  private void Render(RegionPtr<ColorBgra32> input, RegionPtr<ColorBgra32> output, Vector<int> offset) {
    Settings.Deconstruct(out var operation, out var filter, out var painter, out var cameraParameters);
    for (int y = 0; y < output.Height && !IsCancelRequested; y++) {
      for (int x = 0; x < output.Width; x++) {
        int magic = operation(cameraParameters.ApplyTo(new Vector<int>(x, y) + offset));
        output[x, y] = filter(magic) ? painter(magic).GetSrgb() : input[x, y];
      }
    }
  }

  protected override void OnRender(IBitmapEffectOutput output) {
    using var source = Environment.GetSourceBitmapBgra32();
    using var sourceLock = source.Lock(output.Bounds);
    var sourceRegion = sourceLock.AsRegionPtr();
    
    var outputLocation = output.Bounds.Location;
    using var outputLock = output.LockBgra32();
    var outputRegion = outputLock.AsRegionPtr();

    Render(sourceRegion, outputRegion, new Vector<int>(outputLocation.X, outputLocation.Y));
  }
  
  protected override void OnSetToken(PropertyBasedEffectConfigToken? newToken) {
    if(newToken == null) return;
    
    Settings.Camera = new Camera(
      Offset: new Vector<int>(
        newToken.GetProperty<Int32Property>(PropertyNames.OffsetX)!.Value, 
        newToken.GetProperty<Int32Property>(PropertyNames.OffsetY)!.Value),
      Zoom: newToken.GetProperty<Int32Property>(PropertyNames.Zoom)!.Value
    );
    
    Settings.Operation = OperationFactory.FromChoice((OperationChoice)newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.Operation)!.Value);
    
    Settings.Filter = (FilterChoice) newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.FilterMode)!.Value switch {
      FilterChoice.IsDivisible => FilterFactory.DivisibleBy(newToken.GetProperty<Int32Property>(PropertyNames.Divisor)!.Value),
      FilterChoice.IsPrime => FilterFactory.IsPrime,
      _ => throw new ArgumentOutOfRangeException(nameof(newToken), "Unknown filter choice!")
    };

    Settings.Painter = newToken.GetProperty<BooleanProperty>(PropertyNames.UseHSV)!.Value
      ? PainterFactory.SineHsvPainter
      : PainterFactory.ConstantColorPainter(newToken.GetProperty<ManagedColorProperty>(PropertyNames.Color)!.Value);
    
    base.OnSetToken(newToken);
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