using catiqueue.PaintDotNet.Plugins.Common;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

internal sealed record Settings : ISettings<Settings> {
  public static Settings Default { get; } = new();
  
  public ManagedColor BackgroundColor { get; init; } = ManagedColor.Create(SrgbColors.Black);
  public ManagedColor FirstColor { get; init; } = ManagedColor.Create(SrgbColors.Red);
  public ManagedColor SecondColor { get; init; } = ManagedColor.Create(SrgbColors.White);
  public Vector<int> Spacer { get; init; } = Vector<int>.One;
  public Vector<int> Size { get; init; } = Vector<int>.One;
  
  public static Settings FromConfigToken(PropertyBasedEffectConfigToken token) => token.ToSettings();
}