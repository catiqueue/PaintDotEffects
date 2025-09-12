using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using Math = catiqueue.PaintDotNet.Plugins.Common.Math;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Trigonometry;

internal sealed class AcosGeneratorExpression(IGeneratorExpression arg) : IGeneratorExpression {
  public Range<float> ValueRange { get; } = new(0, MathF.PI);
  public float Evaluate(float x, float y) => MathF.Acos(Math.Scale(arg.Evaluate(x, y), arg.ValueRange, Range<float>.Mathematic));
}
