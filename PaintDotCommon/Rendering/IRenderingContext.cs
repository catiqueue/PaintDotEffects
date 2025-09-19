using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.Common.Rendering;

public interface IRenderingContext<TPixel> where TPixel : struct {
  Bounds<int> DrawingArea => Destination.Bounds;
  Bounds<int> RealArea => Source.Bounds; 

  IReadonlyCanvas<TPixel> Source { get; }
  ICanvas<TPixel> Destination { get; }
  
  // absolute coordinate access
  void DrawFromSource(Vector<int> pos);
  void Draw(Vector<int> pos, TPixel value);
  TPixel Read(Vector<int> pos);
}