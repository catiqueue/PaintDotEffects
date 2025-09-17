using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Usability;

internal sealed class ExpressionContainer {
  public IGeneratorExpression R { get; private set; } = ConstantGeneratorExpression.Zero;
  public IGeneratorExpression G { get; private set; } = ConstantGeneratorExpression.Zero;
  public IGeneratorExpression B { get; private set; } = ConstantGeneratorExpression.Zero;
  public IGeneratorExpression A { get; private set; } = ConstantGeneratorExpression.Zero;
  
  public IGeneratorExpression H => R;
  public IGeneratorExpression S => G;
  public IGeneratorExpression V => B;
  
  public void Regenerate(ExpressionFactoryContext context) {
    R = ExpressionFactory.CreateExpression(context);
    G = ExpressionFactory.CreateExpression(context);
    B = ExpressionFactory.CreateExpression(context);
    A = ExpressionFactory.CreateExpression(context);
  }
}
