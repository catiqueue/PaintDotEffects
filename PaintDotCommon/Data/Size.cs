using System.Numerics;

namespace catiqueue.PaintDotNet.Plugins.Common.Data;

public readonly record struct Size<T>(T Width, T Height) where T : INumber<T> {
  public Range<T> WidthRange => new(T.Zero, Width);
  public Range<T> HeightRange => new(T.Zero, Height);
  // this can and will overflow (: (sorry)
  public T Area => Width * Height;
  
  public Size<TOther> As<TOther>() where TOther : INumber<TOther> =>
    new(TOther.CreateChecked(Width), TOther.CreateChecked(Height));
  
  public static Size<T> operator +(Size<T> that, T scalar) => new(that.Width + scalar, that.Height + scalar);
  public static Size<T> operator -(Size<T> that, T scalar) => new(that.Width - scalar, that.Height - scalar);
  public static Size<T> operator *(Size<T> that, T scalar) => new(that.Width * scalar, that.Height * scalar);
  public static Size<T> operator /(Size<T> that, T scalar) => new(that.Width / scalar, that.Height / scalar);
};