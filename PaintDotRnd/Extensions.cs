using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotRnd;

internal static class Extensions {
  public static PropertyCollection GetPropertyCollection(this Plugin plugin) => new([
    new Int32Property(Plugin.PropertyNames.Zoom, Settings.Default.Zoom, 1, 128),
    // it makes sense to limit the precision to 256, but it doesn't make a noticeable difference
    // when this value is bigger than, like, 10, so I think it's fine to limit it to 32.
    new Int32Property(Plugin.PropertyNames.Precision, Settings.Default.Precision, 2, 32),
    new Int32Property(Plugin.PropertyNames.Seed, Settings.Default.Seed, 0, int.MaxValue)
  ], []);
  
  public static ControlInfo GetConfigUI(this Plugin plugin, PropertyCollection properties) {
    ControlInfo ConfigUI = PropertyBasedBitmapEffect.CreateDefaultConfigUI(properties);

    ConfigUI.SetPropertyControlValue(Plugin.PropertyNames.Zoom, ControlInfoPropertyNames.DisplayName, "Zoom");

    ConfigUI.SetPropertyControlValue(Plugin.PropertyNames.Precision, ControlInfoPropertyNames.DisplayName, "Precision");

    ConfigUI.SetPropertyControlValue(Plugin.PropertyNames.Seed, ControlInfoPropertyNames.DisplayName, string.Empty);
    ConfigUI.SetPropertyControlType(Plugin.PropertyNames.Seed, PropertyControlType.IncrementButton);
    ConfigUI.SetPropertyControlValue(Plugin.PropertyNames.Seed, ControlInfoPropertyNames.ButtonText, "Reseed");

    return ConfigUI;
  }
  
  public static Settings ToSettings(this PropertyBasedEffectConfigToken token) => new(
    Seed: token.GetProperty<Int32Property>(Plugin.PropertyNames.Seed)!.Value,
    Precision: token.GetProperty<Int32Property>(Plugin.PropertyNames.Precision)!.Value,
    Zoom: token.GetProperty<Int32Property>(Plugin.PropertyNames.Zoom)!.Value
  );
}