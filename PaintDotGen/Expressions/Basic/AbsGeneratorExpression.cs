using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;

internal sealed class AbsGeneratorExpression(IGeneratorExpression arg) : IGeneratorExpression {
  public Range<float> ValueRange => arg.ValueRange.Abs();
  public float Evaluate(float x, float y) => MathF.Abs(arg.Evaluate(x, y));
}