using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using static catiqueue.PaintDotNet.Plugins.PaintDotBasket.Math;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

[PluginSupportInfo(typeof(PluginSupportInfo))]
internal sealed class Plugin() : PropertyBasedBitmapEffect(Info.DisplayName, PluginSupportInfo.Icon, PluginSupportInfo.SubMenu, BitmapEffectOptionsFactory.Create() with { IsConfigurable = true }) {
  internal enum PropertyNames { BackgroundColor, FirstColor, SecondColor, XSpacer, YSpacer, XSize, YSize }
  private static readonly PluginSupportInfo Info = new();
  private Settings _settings = new();

  private void Render(RegionPtr<ColorBgra32> input, RegionPtr<ColorBgra32> output, Vector<int> offset) {
    _settings.Deconstruct(out var backgroundColor, out var firstColor, out var secondColor, out var spacer, out var size);
    
    for (int y = 0; y < output.Height && !IsCancelRequested; y++) {
      for (int x = 0; x < output.Width; x++) {
        // I can't remember why I'm adding ones everywhere
        output[x, y] = Filter(new Vector<int>(x + offset.X, y + offset.Y) / _settings.Size, _settings.Spacer + Vector<int>.One)
          ? IsJunction(new Vector<int>(x + offset.X, y + offset.Y) / _settings.Size, _settings.Spacer + Vector<int>.One)
            ? IsLineEvenCombined(new Vector<int>(x + offset.X, y + offset.Y) / _settings.Size, _settings.Spacer + Vector<int>.One)
              ? _settings.FirstColor.GetSrgb()
              : _settings.SecondColor.GetSrgb()
            : IsLineActive((y + offset.Y) / _settings.Size.Y, _settings.Spacer.Y + 1)
              ? _settings.SecondColor.GetSrgb()
              : _settings.FirstColor.GetSrgb()
          : _settings.BackgroundColor.GetSrgb();
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
}