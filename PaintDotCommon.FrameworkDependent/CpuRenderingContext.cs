using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;

public sealed class CpuRenderingContext<TPixel>(
  RegionPtr<TPixel> source, RegionPtr<TPixel> output, Vector<int> outputBlockOffset) 
  : IRenderingContext<TPixel> 
  where TPixel : unmanaged, INaturalPixelInfo 
{
  public Bounds<int> DrawingArea { get; } = new(outputBlockOffset, new Size<int>(output.Width, output.Height));
  public Bounds<int> RealArea { get; } = new(Vector<int>.Zero, new Size<int>(source.Width, source.Height));
  
  public void DrawFromSource(Vector<int> pos) {
    if(!DrawingArea.Contains(pos)) throw new System.ArgumentOutOfRangeException(nameof(pos));
    Draw(pos, Read(pos));
  }

  public void Draw(Vector<int> pos, TPixel value) {
    if(!DrawingArea.Contains(pos)) throw new System.ArgumentOutOfRangeException(nameof(pos));
    output[pos.X - DrawingArea.Position.X, pos.Y - DrawingArea.Position.Y] = value;
  }

  public TPixel Read(Vector<int> pos) => DrawingArea.Contains(pos)
      ? source[pos.X, pos.Y]
      : throw new System.ArgumentOutOfRangeException(nameof(pos));
}