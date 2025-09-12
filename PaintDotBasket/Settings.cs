using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

internal sealed record Settings {
  public ManagedColor BackgroundColor { get; init; } = ManagedColor.Create(SrgbColors.Black);
  public ManagedColor FirstColor { get; init; } = ManagedColor.Create(SrgbColors.Red);
  public ManagedColor SecondColor { get; init; } = ManagedColor.Create(SrgbColors.White);
  public Vector<int> Spacer { get; init; } = Vector<int>.One;
  public Vector<int> Size { get; init; } = Vector<int>.One;
  public void Deconstruct(out ManagedColor backgroundColor, out ManagedColor firstColor, out ManagedColor secondColor, out Vector<int> spacer, out Vector<int> size) 
    => (backgroundColor, firstColor, secondColor, spacer, size) = (BackgroundColor, FirstColor, SecondColor, Spacer, Size);
}