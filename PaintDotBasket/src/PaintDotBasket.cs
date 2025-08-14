using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using static LolK.PaintDotNet.Plugins.Math;
using IVec2 = PaintDotNet.Rendering.Vector2Int32;

namespace LolK.PaintDotNet.Plugins;

internal static class Constants {
  public const string EffectName = "Basket";
  public const string Description = "Basket pattern effect for Paint.NET";
  public const string Version = "1.2.0";
  public const string Author = "catiqueue";
  public const string Culture = "en-US";
  public const string Website = "https://github.com/catiqueue";
  public const string Copyright = $"Copyright © {Author} 2025";
}

internal sealed record BasketSettings {
  public ManagedColor BackgroundColor { get; init; } = ManagedColor.Create(SrgbColors.Black);
  public ManagedColor FirstColor { get; init; } = ManagedColor.Create(SrgbColors.Red);
  public ManagedColor SecondColor { get; init; } = ManagedColor.Create(SrgbColors.White);
  public IVec2 Spacer { get; init; } = new(1, 1);
  public IVec2 Size { get; init; } = new(1, 1);
}

[PluginSupportInfo(typeof(PluginSupportInfo))]
internal sealed class PaintDotBasket() : PropertyBasedBitmapEffect(Info.DisplayName, PluginSupportInfo.Icon, PluginSupportInfo.SubMenu, BitmapEffectOptionsFactory.Create() with { IsConfigurable = true }) {
  internal enum PropertyNames { BackgroundColor, FirstColor, SecondColor, XSpacer, YSpacer, XSize, YSize }
  private static readonly PluginSupportInfo Info = new();
  private BasketSettings _settings = new();

  protected override PropertyCollection OnCreatePropertyCollection() => new([
    new ManagedColorProperty(PropertyNames.BackgroundColor, ManagedColor.Create(SrgbColors.Transparent)),
    new ManagedColorProperty(PropertyNames.FirstColor, Environment.PrimaryColor),
    new ManagedColorProperty(PropertyNames.SecondColor, Environment.SecondaryColor),
    new Int32Property(PropertyNames.XSpacer, 1, 1, 255),
    new Int32Property(PropertyNames.YSpacer, 1, 1, 255),
    new Int32Property(PropertyNames.XSize, 1, 1, 255),
    new Int32Property(PropertyNames.YSize, 1, 1, 255)
  ]);

  protected override void OnSetToken(PropertyBasedEffectConfigToken? newToken) {
    _settings = newToken!.ToBasketSettings();
    base.OnSetToken(newToken);
  }
  
  protected override ControlInfo OnCreateConfigUI(PropertyCollection properties) {
    ControlInfo configUi = CreateDefaultConfigUI(properties);

    configUi.SetPropertyControlValue(PropertyNames.FirstColor, ControlInfoPropertyNames.DisplayName, "First color");
    configUi.SetPropertyControlType(PropertyNames.FirstColor, PropertyControlType.ColorWheel);
    configUi.SetPropertyControlValue(PropertyNames.FirstColor, ControlInfoPropertyNames.ShowResetButton, false);

    configUi.SetPropertyControlValue(PropertyNames.SecondColor, ControlInfoPropertyNames.DisplayName, "Second color");
    configUi.SetPropertyControlType(PropertyNames.SecondColor, PropertyControlType.ColorWheel);
    configUi.SetPropertyControlValue(PropertyNames.SecondColor, ControlInfoPropertyNames.ShowResetButton, false);

    configUi.SetPropertyControlValue(PropertyNames.BackgroundColor, ControlInfoPropertyNames.DisplayName, "Background color");
    configUi.SetPropertyControlType(PropertyNames.BackgroundColor, PropertyControlType.ColorWheel);
    configUi.SetPropertyControlValue(PropertyNames.BackgroundColor, ControlInfoPropertyNames.ShowResetButton, false);

    configUi.SetPropertyControlValue(PropertyNames.XSpacer, ControlInfoPropertyNames.DisplayName, "Vertical spacing");
    configUi.SetPropertyControlValue(PropertyNames.YSpacer, ControlInfoPropertyNames.DisplayName, "Horizontal spacing");

    configUi.SetPropertyControlValue(PropertyNames.XSize, ControlInfoPropertyNames.DisplayName, "V-lines size");
    configUi.SetPropertyControlValue(PropertyNames.YSize, ControlInfoPropertyNames.DisplayName, "H-lines size");

    return configUi;
  }

  protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection properties) {
    properties[ControlInfoPropertyNames.WindowHelpContentType]!.Value = WindowHelpContentType.PlainText;
    properties[ControlInfoPropertyNames.WindowHelpContent]!.Value = string.Format(PluginSupportInfo.Culture, "{0}\n{1} v{2}\n{3}\nAll rights reserved.", Info.Copyright, Info.DisplayName, Info.Version, Info.Author);

    base.OnCustomizeConfigUIWindowProperties(properties);
  }

  protected override void OnRender(IBitmapEffectOutput output) {
    using var outputLock = output.LockBgra32();
    var outputRegion = outputLock.AsRegionPtr();
    var pos = new IVec2();

    for (pos.Y = 0; pos.Y < outputRegion.Height && !IsCancelRequested; pos.Y++) {
      for (pos.X = 0; pos.X < outputRegion.Width; pos.X++) {
        // I can't remember why I'm adding ones everywhere
        outputRegion[pos.X, pos.Y] = Filter(pos / _settings.Size, _settings.Spacer + IVec2.One)
          ? IsJunction(pos / _settings.Size, _settings.Spacer + IVec2.One)
            ? IsLineEvenCombined(pos / _settings.Size, _settings.Spacer + IVec2.One)
              ? _settings.FirstColor.GetSrgb()
              : _settings.SecondColor.GetSrgb()
            : IsLineActive(pos.Y, _settings.Spacer.Y + 1)
              ? _settings.SecondColor.GetSrgb()
              : _settings.FirstColor.GetSrgb()
          : _settings.BackgroundColor.GetSrgb();
      }
    }
  }
}

internal static class Math {
  public static bool IsLineActive(int num, int spacer) => num % spacer == 0;
  private static bool IsLineEven(int num, int spacer) => num % (spacer * 2) == 0;
  // I can't remember why I'm computing this like that
  public static bool IsLineEvenCombined(IVec2 pos, IVec2 spacer) => IsLineEven(pos.X, spacer.X) ^ IsLineEven(pos.Y, spacer.Y);
  public static bool Filter(IVec2 pos, IVec2 spacer) => IsLineActive(pos.X, spacer.X) || IsLineActive(pos.Y, spacer.Y);
  public static bool IsJunction(IVec2 pos, IVec2 spacer) => IsLineActive(pos.X, spacer.X) && IsLineActive(pos.Y, spacer.Y);
}

// I don't want this
internal sealed class PluginSupportInfo : IPluginSupportInfo {
  public string Author => Constants.Author;
  public string Copyright => Constants.Copyright;
  public string DisplayName => Constants.EffectName;
  public Version Version => new(Constants.Version);
  public Uri WebsiteUri => new(Constants.Website);
  
  public static CultureInfo Culture => new(Constants.Culture);
  public static Image Icon { get; } = Image.FromStream(new MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQBAMAAADt3eJSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJUExURf////8AAAAAAJqVApEAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAXSURBVBjTY2AQgEIhKIALIETopkZICADJXwaBQIZGVgAAAABJRU5ErkJggg=="), 0, 151), true);
  public static string SubMenu => SubmenuNames.Render;
}

internal static class PropertyBasedEffectConfigTokenExtensions {
  public static BasketSettings ToBasketSettings(this PropertyBasedEffectConfigToken token) => new() {
    BackgroundColor = token.GetProperty<ManagedColorProperty>(PaintDotBasket.PropertyNames.BackgroundColor)!.Value,
    FirstColor = token.GetProperty<ManagedColorProperty>(PaintDotBasket.PropertyNames.FirstColor)!.Value,
    SecondColor = token.GetProperty<ManagedColorProperty>(PaintDotBasket.PropertyNames.SecondColor)!.Value,
    Spacer = new IVec2(x: token.GetProperty<Int32Property>(PaintDotBasket.PropertyNames.XSpacer)!.Value, 
      y:token.GetProperty<Int32Property>(PaintDotBasket.PropertyNames.YSpacer)!.Value),
    Size = new IVec2(x: token.GetProperty<Int32Property>(PaintDotBasket.PropertyNames.XSize)!.Value, 
      y: token.GetProperty<Int32Property>(PaintDotBasket.PropertyNames.YSize)!.Value)
  };
}