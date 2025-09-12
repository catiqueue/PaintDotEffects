using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Usability;

internal sealed class ExpressionContainer {
  public ExpressionContainer() {
    for(int i = 0; i < Expressions.Length; ++i) {
      Expressions[i] = new ConstantGeneratorExpression(1f, Range<float>.Normalized);
    }
  }
    
  private IGeneratorExpression[] Expressions { get; } = new IGeneratorExpression[4];

  public IGeneratorExpression R => Expressions[0];
  public IGeneratorExpression G => Expressions[1];
  public IGeneratorExpression B => Expressions[2];
  public IGeneratorExpression A => Expressions[3];
  
  public IGeneratorExpression H => Expressions[0];
  public IGeneratorExpression S => Expressions[1];
  public IGeneratorExpression V => Expressions[2];
  
  public void Regenerate(ExpressionFactoryContext context) {
    for (int i = 0; i < Expressions.Length; ++i) {
      Expressions[i] = ExpressionFactory.CreateExpression(context);
    }
  }
}