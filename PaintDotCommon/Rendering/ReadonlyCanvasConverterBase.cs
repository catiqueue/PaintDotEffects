using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.Common.Rendering;

// this can also implement TRead Convert(TProduced value), so I can write as well,
// but it's not required by my use case, so I'll leave it for when I need it.
public abstract class ReadonlyCanvasConverterBase<TRead, TProduced>(IReadonlyCanvas<TRead> source) 
  : IReadonlyCanvas<TProduced> where TProduced : struct where TRead : struct {
  public Bounds<int> Bounds => source.Bounds;
  
  protected abstract TProduced Convert(TRead value);
  
  public TProduced Read(Vector<int> pos)
    // I think source canvas should handle the out-of-bounds read here.
    => Convert(source.Read(pos));
}