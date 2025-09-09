namespace catiqueue.PaintDotNet.Plugins.Common.Data;

// more to come
public readonly record struct Vector2I(int X, int Y) {
  public static Vector2I Zero { get; } = new(0, 0);
  
  public static Vector2I operator +(Vector2I a, Vector2I b) => new(a.X + b.X, a.Y + b.Y);
  public static Vector2I operator +(Vector2I vec, int scalar) => new(vec.X + scalar, vec.Y + scalar);
  
  public static Vector2I operator -(Vector2I a, Vector2I b) => new(a.X - b.X, a.Y - b.Y);
  public static Vector2I operator -(Vector2I vec, int scalar) => new(vec.X - scalar, vec.Y - scalar);
  
  public static Vector2I operator *(Vector2I a, Vector2I b) => new(a.X * b.X, a.Y * b.Y);
  public static Vector2I operator *(Vector2I vec, int scalar) => new(vec.X * scalar, vec.Y * scalar);
  
  public static Vector2I operator /(Vector2I a, Vector2I b) => new(a.X / b.X, a.Y / b.Y);
  public static Vector2I operator /(Vector2I vec, int scalar) => new(vec.X / scalar, vec.Y / scalar);
  
  public static Vector2I operator %(Vector2I a, Vector2I b) => new(a.X % b.X, a.Y % b.Y);
};
