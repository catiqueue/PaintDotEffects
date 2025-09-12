using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;

internal enum Parameter { X, Y }

internal sealed class ParameterGeneratorExpression(Parameter param, Size<int> canvasSize, bool normalize) : IGeneratorExpression {
  public Range<float> ValueRange
    => normalize
      ? Range<float>.Normalized
      : param switch {
        Parameter.X => canvasSize.WidthRange.As<float>(),
        Parameter.Y => canvasSize.HeightRange.As<float>(),
        _ => throw new ArgumentOutOfRangeException(nameof(param))
      };

  public float Evaluate(float x, float y) => param switch {
    Parameter.X => normalize ? x / canvasSize.Width  : x,
    Parameter.Y => normalize ? y / canvasSize.Height : y,
    _ => throw new ArgumentOutOfRangeException(nameof(param))
  };
}