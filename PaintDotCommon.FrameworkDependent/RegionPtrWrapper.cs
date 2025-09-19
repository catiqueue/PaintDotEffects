using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet;

namespace catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;

public class RegionPtrWrapper<T>(RegionPtr<T> region, Vector<int> regionOffset) : ICanvas<T> where T : unmanaged {
  public Bounds<int> Bounds { get; } = new(regionOffset, new Size<int>(region.Width, region.Height));
  
  public T Read(Vector<int> pos) {
    if(!Bounds.Contains(pos)) throw new System.ArgumentOutOfRangeException(nameof(pos));
    pos -= regionOffset;
    return region[pos.X, pos.Y];
  }

  public void Draw(Vector<int> pos, T value) {
    if(!Bounds.Contains(pos)) throw new System.ArgumentOutOfRangeException(nameof(pos));
    pos -= regionOffset;
    region[pos.X, pos.Y] = value;
  }
}