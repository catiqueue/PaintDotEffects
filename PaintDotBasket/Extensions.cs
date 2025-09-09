using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.Effects;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

internal static class Extensions {
  public static Settings ToBasketSettings(this PropertyBasedEffectConfigToken token) => new() {
    BackgroundColor = token.GetProperty<ManagedColorProperty>(Plugin.PropertyNames.BackgroundColor)!.Value,
    FirstColor = token.GetProperty<ManagedColorProperty>(Plugin.PropertyNames.FirstColor)!.Value,
    SecondColor = token.GetProperty<ManagedColorProperty>(Plugin.PropertyNames.SecondColor)!.Value,
    Spacer = new Vector2I(X: token.GetProperty<Int32Property>(Plugin.PropertyNames.XSpacer)!.Value, 
                          Y:token.GetProperty<Int32Property>(Plugin.PropertyNames.YSpacer)!.Value),
    Size = new Vector2I(X: token.GetProperty<Int32Property>(Plugin.PropertyNames.XSize)!.Value, 
                        Y: token.GetProperty<Int32Property>(Plugin.PropertyNames.YSize)!.Value)
  };
}