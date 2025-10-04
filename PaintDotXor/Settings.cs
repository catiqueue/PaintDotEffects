using catiqueue.PaintDotNet.Plugins.Common;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXor;

internal sealed record Settings : ISettings<Settings> {
  public static Settings Default { get; } = new();
  
  public Operation Operation { get; set; } = OperationFactory.XOR;
  public Filter Filter { get; set; } = FilterFactory.IsPrime;
  public Painter Painter { get; set; } = PainterFactory.SineHsvPainter;
  public Camera Camera { get; set; } = Camera.Default;
  
  public void Deconstruct(out Operation operation, out Filter filter, out Painter painter, out Camera camera) 
    => (operation, filter, painter, camera) = (Operation, Filter, Painter, Camera);
  
  public static Settings FromConfigToken(PropertyBasedEffectConfigToken token) => token.ToSettings();
}