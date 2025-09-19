using catiqueue.PaintDotNet.Plugins.Common;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using static catiqueue.PaintDotNet.Plugins.Common.BoundsHandling;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal enum BoundsHandlingAction { None = 0, Clamp, Wrap, Mirror, ReturnZero, ReturnOne }

internal class EcaBoundsHandlingCanvasReader(IReadonlyCanvas<EcaPoint> source, BoundsHandlingAction mode) : IReadonlyCanvas<EcaPoint> {
  public Bounds<int> Bounds => source.Bounds;
  
  // actually, I don't need to handle bounds for Y, but it's just cleaner,
  // and if it's really slow, I can always come back later.
  public EcaPoint Read(Vector<int> pos) => Bounds.Contains(pos) ? source.Read(pos) : HandleBounds(pos);

  private EcaPoint HandleBounds(Vector<int> pos) => mode switch {
    BoundsHandlingAction.ReturnOne => EcaPoint.Active(EcaPointDescriptor.None),
    BoundsHandlingAction.ReturnZero => EcaPoint.Inactive(EcaPointDescriptor.None),
    _ => source.Read(HandleFor(pos, source.Bounds.Size, (BoundsHandlingMode) mode))
  };
}