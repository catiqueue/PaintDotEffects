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
    R = ExpressionFactory.CreateExpression(context.XorSeed(0x55555555));
    G = ExpressionFactory.CreateExpression(context.XorSeed(0x33333333));
    B = ExpressionFactory.CreateExpression(context.XorSeed(0x0F0F0F0F));
    A = ExpressionFactory.CreateExpression(context.XorSeed(0x00FF00FF));
  }
}

file static class Extensions {
  public static ExpressionFactoryContext XorSeed(this ExpressionFactoryContext context, int value) 
    => new(context.Options with { Seed = context.Options.Seed ^ value }, context.Parameters);
}