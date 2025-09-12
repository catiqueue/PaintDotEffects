using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen;

internal static class PluginExtensions {
  // this can be simplified? 
  public static PropertyCollection GetPropertyCollection(this Plugin plugin) => new([
    new Int32Property(PropertyNames.Seed, Settings.Default.Seed, int.MinValue, int.MaxValue),
    new Int32Property(PropertyNames.Complexity, Settings.Default.Complexity, 2, 16),
    new Int32Property(PropertyNames.ConstantRange, Settings.Default.ConstantRange, 128, 16384),
    new DoubleProperty(PropertyNames.ImaginedCanvasRescaleFactor, Settings.Default.RescaleFactor, 1d, 128d),
    new BooleanProperty(PropertyNames.Normalized, Settings.Default.Normalized),
    new BooleanProperty(PropertyNames.UseHsv, Settings.Default.UseHsv),
  ]);
  
  public static ControlInfo GetDefaultConfigUI(this Plugin plugin, PropertyCollection properties) {
    var configUi = PropertyBasedBitmapEffect.CreateDefaultConfigUI(properties);

    configUi.SetPropertyControlValue(PropertyNames.Seed, ControlInfoPropertyNames.DisplayName, "Seed");
    configUi.SetPropertyControlValue(PropertyNames.Complexity, ControlInfoPropertyNames.DisplayName, "Complexity (tree depth)");
    configUi.SetPropertyControlValue(PropertyNames.ConstantRange, ControlInfoPropertyNames.DisplayName, "Constant value range (-X, X)");
    configUi.SetPropertyControlValue(PropertyNames.ImaginedCanvasRescaleFactor, ControlInfoPropertyNames.DisplayName, "Scale factor (doesn't affect performance)");
    configUi.SetPropertyControlValue(PropertyNames.Normalized, ControlInfoPropertyNames.DisplayName, "Normalize input values (mostly smooth results)");
    configUi.SetPropertyControlValue(PropertyNames.UseHsv, ControlInfoPropertyNames.DisplayName, "Use HSV color interpretation instead of RGB");

    return configUi;
  }
  
  public static Settings ToSettings(this PropertyBasedEffectConfigToken token) => new() {
    Seed = token.GetProperty<Int32Property>(PropertyNames.Seed)!.Value,
    Complexity = token.GetProperty<Int32Property>(PropertyNames.Complexity)!.Value,
    Normalized = token.GetProperty<BooleanProperty>(PropertyNames.Normalized)!.Value,
    ConstantRange = token.GetProperty<Int32Property>(PropertyNames.ConstantRange)!.Value,
    RescaleFactor = (float) token.GetProperty<DoubleProperty>(PropertyNames.ImaginedCanvasRescaleFactor)!.Value,
    UseHsv = token.GetProperty<BooleanProperty>(PropertyNames.UseHsv)!.Value,
  };
}