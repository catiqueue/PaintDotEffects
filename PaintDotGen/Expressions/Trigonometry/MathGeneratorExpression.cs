using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Trigonometry;

internal sealed class MathGeneratorExpression(Func<float, float> function, IGeneratorExpression arg, Range<float> valueRange) : IGeneratorExpression {
  public Range<float> ValueRange { get; } = valueRange;
  public float Evaluate(float x, float y) => function(arg.Evaluate(x, y));
}