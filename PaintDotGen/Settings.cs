using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen;

internal readonly record struct Settings(int Seed, int Complexity, bool Normalized = false, int ConstantRange = 1024, float RescaleFactor = 1f, bool UseHsv = true) : ISettings<Settings> {
  public static Settings Default { get; } = new(-1, 4);
  public static Settings FromConfigToken(PropertyBasedEffectConfigToken token) 
    => token.ToSettings();
};