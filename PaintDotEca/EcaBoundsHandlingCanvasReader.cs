using catiqueue.PaintDotNet.Plugins.Common;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using static catiqueue.PaintDotNet.Plugins.Common.BoundsHandling;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal enum EcaBoundsHandlingMode { Clamp, Wrap, Mirror, ReturnZero, ReturnOne }

internal class EcaBoundsHandlingCanvasReader(IReadonlyCanvas<EcaPoint> source, EcaBoundsHandlingMode mode) : IReadonlyCanvas<EcaPoint> {
  public Bounds<int> Bounds => source.Bounds;
  
  public EcaPoint Read(Vector<int> pos) => pos.X >= source.Bounds.Left
                                           && pos.X < source.Bounds.Right ? source.Read(pos) : HandleBounds(pos);

  private EcaPoint HandleBounds(Vector<int> pos) => mode switch {
    EcaBoundsHandlingMode.ReturnOne => EcaPoint.Active(EcaPointDescriptor.None),
    EcaBoundsHandlingMode.ReturnZero => EcaPoint.Inactive(EcaPointDescriptor.None),
    _ => source.Read(HandleFor(pos, source.Bounds.Size, (BoundsHandlingMode) mode))
  };
}