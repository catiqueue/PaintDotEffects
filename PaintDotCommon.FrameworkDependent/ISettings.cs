using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.Common;

public interface ISettings<out TSelf> where TSelf : class, ISettings<TSelf> {
  public static abstract TSelf Default { get; }
  public static abstract TSelf FromConfigToken(PropertyBasedEffectConfigToken token);
};