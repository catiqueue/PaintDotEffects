using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;

public sealed class CpuRenderingContext<TPixel>(
  RegionPtr<TPixel> sourceRegion, RegionPtr<TPixel> destinationRegion, Vector<int> outputBlockOffset) : IRenderingContext<TPixel> 
  where TPixel : unmanaged, INaturalPixelInfo 
{
  public IReadonlyCanvas<TPixel> Source { get; } = new RegionPtrWrapper<TPixel>(sourceRegion, Vector<int>.Zero);
  public ICanvas<TPixel> Destination { get; } = new RegionPtrWrapper<TPixel>(destinationRegion, outputBlockOffset);
  
  public void DrawFromSource(Vector<int> pos) => Destination[pos] = Source[pos];
  public void Draw(Vector<int> pos, TPixel value) => Destination[pos] = value;
  public TPixel Read(Vector<int> pos) => Source[pos];
}