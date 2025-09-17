using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

internal static class Extensions {
  public static PropertyCollection GetPropertyCollection(this Plugin plugin) => new([
    new ManagedColorProperty(Plugin.PropertyNames.BackgroundColor, Settings.Default.BackgroundColor),
    new ManagedColorProperty(Plugin.PropertyNames.FirstColor, Settings.Default.FirstColor),
    new ManagedColorProperty(Plugin.PropertyNames.SecondColor, Settings.Default.SecondColor),
    new Int32Property(Plugin.PropertyNames.XSpacer, Settings.Default.Spacer.X, 1, 255),
    new Int32Property(Plugin.PropertyNames.YSpacer, Settings.Default.Spacer.Y, 1, 255),
    new Int32Property(Plugin.PropertyNames.XSize, Settings.Default.Size.X, 1, 255),
    new Int32Property(Plugin.PropertyNames.YSize, Settings.Default.Size.Y, 1, 255)
  ]);
  
  public static ControlInfo GetConfigUI(this Plugin plugin, PropertyCollection properties) {
    ControlInfo configUi = PropertyBasedBitmapEffect.CreateDefaultConfigUI(properties);

    configUi.SetPropertyControlValue(Plugin.PropertyNames.FirstColor, ControlInfoPropertyNames.DisplayName, "First color");
    configUi.SetPropertyControlType(Plugin.PropertyNames.FirstColor, PropertyControlType.ColorWheel);
    configUi.SetPropertyControlValue(Plugin.PropertyNames.FirstColor, ControlInfoPropertyNames.ShowResetButton, false);

    configUi.SetPropertyControlValue(Plugin.PropertyNames.SecondColor, ControlInfoPropertyNames.DisplayName, "Second color");
    configUi.SetPropertyControlType(Plugin.PropertyNames.SecondColor, PropertyControlType.ColorWheel);
    configUi.SetPropertyControlValue(Plugin.PropertyNames.SecondColor, ControlInfoPropertyNames.ShowResetButton, false);

    configUi.SetPropertyControlValue(Plugin.PropertyNames.BackgroundColor, ControlInfoPropertyNames.DisplayName, "Background color");
    configUi.SetPropertyControlType(Plugin.PropertyNames.BackgroundColor, PropertyControlType.ColorWheel);
    configUi.SetPropertyControlValue(Plugin.PropertyNames.BackgroundColor, ControlInfoPropertyNames.ShowResetButton, false);

    configUi.SetPropertyControlValue(Plugin.PropertyNames.XSpacer, ControlInfoPropertyNames.DisplayName, "Vertical spacing");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.YSpacer, ControlInfoPropertyNames.DisplayName, "Horizontal spacing");

    configUi.SetPropertyControlValue(Plugin.PropertyNames.XSize, ControlInfoPropertyNames.DisplayName, "V-lines size");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.YSize, ControlInfoPropertyNames.DisplayName, "H-lines size");

    return configUi;
  }
  
  public static Settings ToSettings(this PropertyBasedEffectConfigToken token) => new() {
    BackgroundColor = token.GetProperty<ManagedColorProperty>(Plugin.PropertyNames.BackgroundColor)!.Value,
    FirstColor = token.GetProperty<ManagedColorProperty>(Plugin.PropertyNames.FirstColor)!.Value,
    SecondColor = token.GetProperty<ManagedColorProperty>(Plugin.PropertyNames.SecondColor)!.Value,
    Spacer = new Vector<int>(X: token.GetProperty<Int32Property>(Plugin.PropertyNames.XSpacer)!.Value, 
                          Y:token.GetProperty<Int32Property>(Plugin.PropertyNames.YSpacer)!.Value),
    Size = new Vector<int>(X: token.GetProperty<Int32Property>(Plugin.PropertyNames.XSize)!.Value, 
                        Y: token.GetProperty<Int32Property>(Plugin.PropertyNames.YSize)!.Value)
  };
}