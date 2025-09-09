using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

internal sealed record Settings {
  public ManagedColor BackgroundColor { get; init; } = ManagedColor.Create(SrgbColors.Black);
  public ManagedColor FirstColor { get; init; } = ManagedColor.Create(SrgbColors.Red);
  public ManagedColor SecondColor { get; init; } = ManagedColor.Create(SrgbColors.White);
  public Vector2I Spacer { get; init; } = Vector2I.One;
  public Vector2I Size { get; init; } = Vector2I.One;
  public void Deconstruct(out ManagedColor backgroundColor, out ManagedColor firstColor, out ManagedColor secondColor, out Vector2I spacer, out Vector2I size) 
    => (backgroundColor, firstColor, secondColor, spacer, size) = (BackgroundColor, FirstColor, SecondColor, Spacer, Size);
}