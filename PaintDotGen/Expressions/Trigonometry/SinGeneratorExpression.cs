using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Trigonometry;

internal sealed class SinGeneratorExpression(IGeneratorExpression arg) : IGeneratorExpression {
  public Range<float> ValueRange { get; } = Range<float>.Mathematic;
  public float Evaluate(float x, float y) => MathF.Sin(arg.Evaluate(x, y));
}


