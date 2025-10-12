using System;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXorTest;

internal sealed record Settings : ICloneable {
  public Operation Operation { get; set; } = OperationFactory.XOR;
  public Filter Filter { get; set; } = FilterFactory.IsPrime;
  public Painter Painter { get; set; } = PainterFactory.SineHsvPainter;
  public Camera Camera { get; set; } = Camera.Default;
  
  object ICloneable.Clone() => new Settings { Operation = Operation, Filter = Filter, Painter = Painter, Camera = Camera };
  
  public void Deconstruct(out Operation operation, out Filter filter, out Painter painter, out Camera camera) 
    => (operation, filter, painter, camera) = (Operation, Filter, Painter, Camera);
}