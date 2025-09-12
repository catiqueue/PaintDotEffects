using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using Math = catiqueue.PaintDotNet.Plugins.Common.Math;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Trigonometry;

internal sealed class AsinGeneratorExpression(IGeneratorExpression arg) : IGeneratorExpression {
  public Range<float> ValueRange { get; } = new(-(MathF.PI / 2), MathF.PI / 2);
  public float Evaluate(float x, float y) => MathF.Asin(Math.Scale(arg.Evaluate(x, y), arg.ValueRange, Range<float>.Mathematic));
}
