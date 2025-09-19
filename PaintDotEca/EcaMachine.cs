using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal sealed class EcaMachine : IReadonlyCanvas<EcaPoint> {
  public BoundsHandlingAction BoundsHandler { get; init; } = BoundsHandlingAction.ReturnOne;
  public byte Rule { get; init; } = 30;
  public Bounds<int> Bounds => _cache.Bounds;
  
  private readonly ICanvas<EcaPoint?> _cache;
  private readonly IReadonlyCanvas<EcaPoint> _safeReader;
  
  public EcaMachine(Size<int> size) {
    _cache = new ArrayCanvas<EcaPoint?>(size);
    _safeReader = new EcaBoundsHandlingCanvasReader(this, BoundsHandler);
  }

  // this copies data on construction, so be careful using this in the plugin.
  // reconstruct only when absolutely necessary.
  public EcaMachine(IReadonlyCanvas<bool> source) {
    _cache = new ArrayCanvas<EcaPoint?>(source.Bounds.Size);
    for (int y = 0; y < Bounds.Size.Height; ++y) {
      for (int x = 0; x < Bounds.Size.Width; ++x) {
        var pos = new Vector<int>(x, y);
        if (source[pos]) 
          _cache[pos] = EcaPoint.Active(EcaPointDescriptor.Pregenerated);
      }
    }
    _safeReader = new EcaBoundsHandlingCanvasReader(this, BoundsHandler);
  }
  
  public EcaPoint Read(Vector<int> pos) {
    if(pos.Y == 0 && _cache[pos] is null) 
      return EcaPoint.Inactive(EcaPointDescriptor.None);
    return _cache[pos] ??= EcaPoint.FromRule(CollectParents(pos), Rule);
  }

  private EcaParents CollectParents(Vector<int> pos) => new(
    _safeReader[new(pos.X - 1, pos.Y - 1)].IsActive, 
    _safeReader[pos with { Y = pos.Y - 1 }].IsActive, 
    _safeReader[new(pos.X + 1, pos.Y - 1)].IsActive);
}