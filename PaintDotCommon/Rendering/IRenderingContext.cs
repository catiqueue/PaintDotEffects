using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.Common.Rendering;

public interface IRenderingContext<TPixel> {
  Bounds<int> DrawingArea { get; }
  Bounds<int> RealArea { get; }
  
  // absolute coordinate access
  void DrawFromSource(Vector<int> pos);
  void Draw(Vector<int> pos, TPixel value);
  TPixel Read(Vector<int> pos);
}