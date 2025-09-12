using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;

internal class ConstantGeneratorExpression(float constant) : IGeneratorExpression {
  public ConstantGeneratorExpression(float constant, Range<float> valueRange) : this(constant) => ValueRange = valueRange;
  public Range<float> ValueRange { get; } = Range<float>.FromSingleValue(constant);
  public float Evaluate(float x, float y) => constant;
}

internal sealed class PiGeneratorExpression() : ConstantGeneratorExpression(MathF.PI);
internal sealed class EGeneratorExpression() : ConstantGeneratorExpression(MathF.E);
internal sealed class TauGeneratorExpression() : ConstantGeneratorExpression(MathF.Tau);