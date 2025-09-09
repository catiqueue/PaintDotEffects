using PaintDotNet.Effects;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotRnd;

internal static class Extensions {
  public static Settings ToSettings(this PropertyBasedEffectConfigToken token) => new(
    Seed: token.GetProperty<Int32Property>(Plugin.PropertyNames.Seed)!.Value,
    Precision: token.GetProperty<Int32Property>(Plugin.PropertyNames.Precision)!.Value,
    Zoom: token.GetProperty<Int32Property>(Plugin.PropertyNames.Zoom)!.Value
  );
}