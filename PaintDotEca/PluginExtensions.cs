using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal static class Extensions {
  internal static PropertyCollection GetPropertyCollection(this Plugin plugin) => new([
    new Int32Property(Plugin.PropertyNames.Rule, Settings.Default.Rule, 0, 255),
    StaticListChoiceProperty.CreateForEnum(Plugin.PropertyNames.BoundsHandling, EcaBoundsHandlingMode.ReturnOne, false),
    StaticListChoiceProperty.CreateForEnum(Plugin.PropertyNames.Painter, EcaPainterChoice.Default, false),
    new BooleanProperty(Plugin.PropertyNames.RespectSourceColor, true),
    StaticListChoiceProperty.CreateForEnum(Plugin.PropertyNames.Activator, EcaActivatorChoice.Transparency, false),
    new DoubleProperty(Plugin.PropertyNames.ActivatorThreshold, 0.5, 0.0, 1.0)
  ]);

  internal static ControlInfo GetConfigUI(this Plugin plugin, PropertyCollection properties) {
    ControlInfo configUi = PropertyBasedBitmapEffect.CreateDefaultConfigUI(properties);
    configUi.SetPropertyControlValue(Plugin.PropertyNames.Rule, ControlInfoPropertyNames.DisplayName, "Rule");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.BoundsHandling, ControlInfoPropertyNames.DisplayName,
      "Bounds Handling Mode");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.BoundsHandling)
      !.SetValueDisplayName(EcaBoundsHandlingMode.Clamp, "Clamp");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.BoundsHandling)
      !.SetValueDisplayName(EcaBoundsHandlingMode.Wrap, "Wrap");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.BoundsHandling)
      !.SetValueDisplayName(EcaBoundsHandlingMode.ReturnZero, "Set False");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.BoundsHandling)
      !.SetValueDisplayName(EcaBoundsHandlingMode.ReturnOne, "Set True");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.Painter, ControlInfoPropertyNames.DisplayName,
      "Color palette");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.Painter)
      !.SetValueDisplayName(EcaPainterChoice.Default, "Default");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.RespectSourceColor, ControlInfoPropertyNames.DisplayName, "");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.RespectSourceColor, ControlInfoPropertyNames.Description, "Respect source color");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.Activator, ControlInfoPropertyNames.DisplayName,
      "Point activator function");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.Activator)
      !.SetValueDisplayName(EcaActivatorChoice.Transparency, "Transparency level");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.Activator)
      !.SetValueDisplayName(EcaActivatorChoice.Grayscale, "Lightness level");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.Activator)
      !.SetValueDisplayName(EcaActivatorChoice.Intensity, "Intensity level");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.ActivatorThreshold, ControlInfoPropertyNames.DisplayName, "Activator threshold");
    return configUi;
  }

  internal static Settings ToSettings(this PropertyBasedEffectConfigToken token) => new() {
    Rule = (byte)token.GetProperty<Int32Property>(Plugin.PropertyNames.Rule)!.Value,
    BoundsHandler =  (EcaBoundsHandlingMode)token.GetProperty<StaticListChoiceProperty>(Plugin.PropertyNames.BoundsHandling)!.Value,
    Painter = EcaPointDescriptorPainters.FromChoice((EcaPainterChoice)token.GetProperty<StaticListChoiceProperty>(Plugin.PropertyNames.Painter)!.Value),
    RespectSourceColor = token.GetProperty<BooleanProperty>(Plugin.PropertyNames.RespectSourceColor)!.Value,
    Activator = EcaPointActivators.FromChoice(
      choice: (EcaActivatorChoice)token.GetProperty<StaticListChoiceProperty>(Plugin.PropertyNames.Activator)!.Value,
      threshold: (byte) (token.GetProperty<DoubleProperty>(Plugin.PropertyNames.ActivatorThreshold)!.Value * byte.MaxValue)),
    
  };
}