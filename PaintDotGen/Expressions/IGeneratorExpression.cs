using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.Collections;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions;

internal interface IGeneratorExpression {
  public Range<float> ValueRange { get; }
  public float Evaluate(float x, float y);
}
