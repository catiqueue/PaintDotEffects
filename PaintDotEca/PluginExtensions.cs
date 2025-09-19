using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal static class Extensions {
  internal static PropertyCollection GetPropertyCollection(this Plugin plugin) => new([
    new Int32Property(Plugin.PropertyNames.Rule, Settings.Default.Rule, 0, 255),
    StaticListChoiceProperty.CreateForEnum(Plugin.PropertyNames.BoundsHandling, BoundsHandlingAction.ReturnOne, false),
  ]);

  internal static ControlInfo GetConfigUI(this Plugin plugin, PropertyCollection properties) {
    ControlInfo configUi = PropertyBasedBitmapEffect.CreateDefaultConfigUI(properties);
    configUi.SetPropertyControlValue(Plugin.PropertyNames.Rule, ControlInfoPropertyNames.DisplayName, "Rule");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.BoundsHandling, ControlInfoPropertyNames.DisplayName,
      "Bounds Handling Mode");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.BoundsHandling)
      !.SetValueDisplayName(BoundsHandlingAction.Clamp, "Wrap");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.BoundsHandling)
      !.SetValueDisplayName(BoundsHandlingAction.Wrap, "Wrap");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.BoundsHandling)
      !.SetValueDisplayName(BoundsHandlingAction.Mirror, "Invert Wrap");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.BoundsHandling)
      !.SetValueDisplayName(BoundsHandlingAction.ReturnZero, "Set False");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.BoundsHandling)
      !.SetValueDisplayName(BoundsHandlingAction.ReturnOne, "Set True");
    return configUi;
  }

  internal static Settings ToSettings(this PropertyBasedEffectConfigToken token) => new() {
    Rule = (byte)token.GetProperty<Int32Property>(Plugin.PropertyNames.Rule)!.Value,
    BoundsHandler =  (BoundsHandlingAction)token.GetProperty<StaticListChoiceProperty>(Plugin.PropertyNames.BoundsHandling)!.Value
  };
}