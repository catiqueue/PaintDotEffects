using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen;

internal static class PluginExtensions {
  // this can be simplified? 
  public static PropertyCollection GetPropertyCollection(this Plugin plugin) => new([
    new Int32Property(Plugin.PropertyNames.Seed, Settings.Default.Seed, int.MinValue, int.MaxValue),
    new Int32Property(Plugin.PropertyNames.Complexity, Settings.Default.Complexity, 2, 16),
    new Int32Property(Plugin.PropertyNames.ConstantRange, Settings.Default.ConstantRange, 128, 16384),
    new DoubleProperty(Plugin.PropertyNames.ImaginedCanvasRescaleFactor, Settings.Default.RescaleFactor, 1d, 128d),
    new BooleanProperty(Plugin.PropertyNames.Normalized, Settings.Default.Normalized),
    StaticListChoiceProperty.CreateForEnum(Plugin.PropertyNames.Interpreter, ExpressionInterpreterChoice.HSV, false),
  ]);
  
  public static ControlInfo GetDefaultConfigUI(this Plugin plugin, PropertyCollection properties) {
    var configUi = PropertyBasedBitmapEffect.CreateDefaultConfigUI(properties);

    configUi.SetPropertyControlValue(Plugin.PropertyNames.Seed, ControlInfoPropertyNames.DisplayName, "Seed");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.Complexity, ControlInfoPropertyNames.DisplayName, "Complexity (tree depth)");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.ConstantRange, ControlInfoPropertyNames.DisplayName, "Constant value range (-X, X)");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.ImaginedCanvasRescaleFactor, ControlInfoPropertyNames.DisplayName, "Scale factor (doesn't affect performance)");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.Normalized, ControlInfoPropertyNames.DisplayName, "Normalize input values (mostly smooth results)");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.Interpreter, ControlInfoPropertyNames.DisplayName, "Interpreter (grayscale only reads the alpha channel)");

    return configUi;
  }
  
  public static Settings ToSettings(this PropertyBasedEffectConfigToken token) => new() {
    Seed = token.GetProperty<Int32Property>(Plugin.PropertyNames.Seed)!.Value,
    Complexity = token.GetProperty<Int32Property>(Plugin.PropertyNames.Complexity)!.Value,
    Normalized = token.GetProperty<BooleanProperty>(Plugin.PropertyNames.Normalized)!.Value,
    ConstantRange = token.GetProperty<Int32Property>(Plugin.PropertyNames.ConstantRange)!.Value,
    RescaleFactor = (float) token.GetProperty<DoubleProperty>(Plugin.PropertyNames.ImaginedCanvasRescaleFactor)!.Value,
    Interpreter = ExpressionInterpreters.FromChoice((ExpressionInterpreterChoice) token.GetProperty<StaticListChoiceProperty>(Plugin.PropertyNames.Interpreter)!.Value)
  };
}