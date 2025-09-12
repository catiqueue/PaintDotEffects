using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;

public interface ISettings<out TSelf> where TSelf : ISettings<TSelf> {
  public static abstract TSelf Default { get; }
  public static abstract TSelf FromConfigToken(PropertyBasedEffectConfigToken token);
};