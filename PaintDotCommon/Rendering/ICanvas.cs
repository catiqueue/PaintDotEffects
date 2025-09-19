using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.Common.Rendering;

public interface IReadonlyCanvas<out T> {
  T this[Vector<int> pos] => Read(pos);
  Bounds<int> Bounds { get; }
  T Read(Vector<int> pos);
}

public interface ICanvas<T> : IReadonlyCanvas<T> {
  new T this[Vector<int> pos] {
    get => Read(pos);
    set => Draw(pos, value);
  }
  
  void Draw(Vector<int> pos, T value);
}