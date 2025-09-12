using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.PaintDotXor.Types;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXor;

internal sealed class Settings {
  public Operation Operation { get; set; } = OperationFactory.XOR;
  public Filter Filter { get; set; } = FilterFactory.IsPrime;
  public Painter Painter { get; set; } = PainterFactory.SineHsvPainter;
  public Camera Camera { get; set; } = Camera.Default;
  public void Deconstruct(out Operation operation, out Filter filter, out Painter painter, out Camera camera) 
    => (operation, filter, painter, camera) = (Operation, Filter, Painter, Camera);
}