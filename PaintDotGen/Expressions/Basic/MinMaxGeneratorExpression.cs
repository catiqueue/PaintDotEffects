using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;

internal sealed class MinGeneratorExpression(IGeneratorExpression arg1, IGeneratorExpression arg2) : IGeneratorExpression {
  public Range<float> ValueRange { get; } = new(MathF.Min(arg1.ValueRange.Min(), arg2.ValueRange.Min()), 
                                                MathF.Max(arg1.ValueRange.Max(), arg2.ValueRange.Max()));
  public float Evaluate(float x, float y) => MathF.Min(arg1.Evaluate(x, y), arg2.Evaluate(x, y));
}

internal sealed class MaxGeneratorExpression(IGeneratorExpression arg1, IGeneratorExpression arg2) : IGeneratorExpression {
  public Range<float> ValueRange { get; } = new(MathF.Min(arg1.ValueRange.Min(), arg2.ValueRange.Min()), 
                                                MathF.Max(arg1.ValueRange.Max(), arg2.ValueRange.Max()));
  public float Evaluate(float x, float y) => MathF.Max(arg1.Evaluate(x, y), arg2.Evaluate(x, y));
}