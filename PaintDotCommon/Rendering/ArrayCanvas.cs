using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.Common.Rendering;

public class ArrayCanvas<T>(Size<int> dimensions) : ICanvas<T> {
  public Bounds<int> Bounds { get; } = new(Vector<int>.Zero, dimensions);
  
  private readonly T[] _data = new T[dimensions.Area];
  
  public T Read(Vector<int> pos) => _data[Math.Array2DAccessTo1D(pos, dimensions.Width)];
  public void Draw(Vector<int> pos, T value) => _data[Math.Array2DAccessTo1D(pos, dimensions.Width)] = value;
}