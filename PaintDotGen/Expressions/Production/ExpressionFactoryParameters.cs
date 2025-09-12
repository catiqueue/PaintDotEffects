using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production;

internal sealed class ExpressionFactoryParameters {
  public required ParameterGeneratorExpression X { get; init; }
  public required ParameterGeneratorExpression Y { get; init; }
}