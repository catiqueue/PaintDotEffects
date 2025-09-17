using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotRnd;

internal sealed record Settings(int Seed, int Precision, int Zoom) : ISettings<Settings> {
  public static Settings Default => new(0, 2, 1);

  public void Deconstruct(out int seed, out int precision, out int zoom) => (seed, precision, zoom) = (Seed, Precision, Zoom);

  public static Settings FromConfigToken(PropertyBasedEffectConfigToken token)
    => token.ToSettings();
};