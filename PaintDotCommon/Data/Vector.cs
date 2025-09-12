using System.Numerics;

namespace catiqueue.PaintDotNet.Plugins.Common.Data;

public readonly record struct Vector<T>(T X, T Y) where T: INumber<T> {
  public static Vector<T> Zero { get; } = new(T.Zero, T.Zero);
  public static Vector<T> One { get; } = new(T.One, T.One);
  
  public Vector<TOther> As<TOther>() where TOther : INumber<TOther> =>
    new(TOther.CreateChecked(X), TOther.CreateChecked(Y));
  
  public static Vector<T> operator +(Vector<T> a, Vector<T> b) => new(a.X + b.X, a.Y + b.Y);
  public static Vector<T> operator +(Vector<T> vec, T scalar) => new(vec.X + scalar, vec.Y + scalar);
  
  public static Vector<T> operator -(Vector<T> a, Vector<T> b) => new(a.X - b.X, a.Y - b.Y);
  public static Vector<T> operator -(Vector<T> vec, T scalar) => new(vec.X - scalar, vec.Y - scalar);
  
  public static Vector<T> operator *(Vector<T> a, Vector<T> b) => new(a.X * b.X, a.Y * b.Y);
  public static Vector<T> operator *(Vector<T> vec, T scalar) => new(vec.X * scalar, vec.Y * scalar);
  
  public static Vector<T> operator /(Vector<T> a, Vector<T> b) => new(a.X / b.X, a.Y / b.Y);
  public static Vector<T> operator /(Vector<T> vec, T scalar) => new(vec.X / scalar, vec.Y / scalar);
  
  public static Vector<T> operator %(Vector<T> a, Vector<T> b) => new(a.X % b.X, a.Y % b.Y);
};
