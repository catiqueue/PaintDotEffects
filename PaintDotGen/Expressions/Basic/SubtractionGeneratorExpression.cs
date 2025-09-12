using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;

internal sealed class SubtractionGeneratorExpression(IGeneratorExpression left, IGeneratorExpression right) : IGeneratorExpression {
  public Range<float> ValueRange => left.ValueRange - right.ValueRange;
  public float Evaluate(float x, float y) => left.Evaluate(x, y) - right.Evaluate(x, y);
}