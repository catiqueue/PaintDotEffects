using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions;

internal class GenericGeneratorExpression(Range<float> valueRange, Func<float, float, float> evalFunc) : IGeneratorExpression {
  
  public GenericGeneratorExpression(Func<IGeneratorExpression, Range<float>> rangeCalculator, Func<IGeneratorExpression, float, float, float> evalFunc,
    IGeneratorExpression arg1) 
    : this(rangeCalculator(arg1), (x, y) => evalFunc(arg1, x, y)) { }
  
  public GenericGeneratorExpression(Func<IGeneratorExpression, IGeneratorExpression, Range<float>> rangeCalculator, Func<IGeneratorExpression, IGeneratorExpression, float, float, float> evalFunc,
    IGeneratorExpression arg1, IGeneratorExpression arg2) 
    : this(rangeCalculator(arg1, arg2), (x, y) => evalFunc(arg1, arg2, x, y)) { }
  
  public GenericGeneratorExpression(Func<IGeneratorExpression, IGeneratorExpression, IGeneratorExpression, Range<float>> rangeCalculator, Func<IGeneratorExpression, IGeneratorExpression, IGeneratorExpression, float, float, float> evalFunc,
    IGeneratorExpression arg1, IGeneratorExpression arg2, IGeneratorExpression arg3) 
    : this(rangeCalculator(arg1, arg2, arg3), (x, y) => evalFunc(arg1, arg2, arg3, x, y)) { }
  
  public Range<float> ValueRange { get; } = valueRange;
  public float Evaluate(float x, float y) => evalFunc(x, y);
}
