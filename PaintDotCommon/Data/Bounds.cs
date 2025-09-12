using System.Numerics;

namespace catiqueue.PaintDotNet.Plugins.Common.Data;


// position is top left
public readonly record struct Bounds<T>(Vector<T> Position, Size<T> Size) where T : INumber<T> {
  public T Top => Position.Y;
  public T Right => Position.X + Size.Width;
  public T Bottom => Position.Y + Size.Height;
  public T Left => Position.X;
  public bool Contains(Vector<T> pos) => pos.X >= Left && pos.X < Right && pos.Y >= Top && pos.Y < Bottom;
};