using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production;

// omg
internal sealed class ExpressionFactoryContextBuilder {
  private int? _seed;
  private int? _complexity;
  private Range<int>? _constantRange;
  private Size<int>? _canvasSize;
  private bool? _normalize;

  public ExpressionFactoryContextBuilder FromSeed(int seed) {
    _seed = seed;
    return this;
  }

  public ExpressionFactoryContextBuilder WithComplexity(int complexity) {
    if(complexity is < 2 or > 24) throw new ArgumentOutOfRangeException(nameof(complexity));
    _complexity = complexity;
    return this;
  }
  
  public ExpressionFactoryContextBuilder WithConstantRange(Range<int> range) {
    if (range.IsBackwards) throw new ArgumentException("The range  is invalid", nameof(range));
    _constantRange = range;
    return this;
  }
  
  public ExpressionFactoryContextBuilder WithCanvasSize(Size<int> size) {
    if(size.Width <= 0 || size.Height <= 0) throw new ArgumentOutOfRangeException(nameof(size));
    _canvasSize = size;
    return this;
  }
  
  public ExpressionFactoryContextBuilder Normalize(bool normalize) {
    _normalize = normalize;
    return this;
  }

  public ExpressionFactoryContext Build() {
    if(_seed is null) throw new InvalidOperationException("Seed is not set");
    if(_complexity is null) throw new InvalidOperationException("Complexity is not set");
    if(_constantRange is null) throw new InvalidOperationException("Constant range is not set");
    if(_canvasSize is null) throw new InvalidOperationException("Canvas size is not set");
    if(_normalize is null) throw new InvalidOperationException("Normalize is not set");

    var parameters = new ExpressionFactoryParameters {
      X = new ParameterGeneratorExpression(Parameter.X, _canvasSize.Value, _normalize.Value),
      Y = new ParameterGeneratorExpression(Parameter.Y, _canvasSize.Value, _normalize.Value),
    };
    
    var options = new ExpressionFactoryOptions {
      Seed = _seed.Value,
      Complexity = _complexity.Value,
      Level = _complexity.Value,
      ConstantRange = _constantRange.Value
    };
    
    return new ExpressionFactoryContext(options, parameters);
  }
  
  public static ExpressionFactoryContext BuildFromSettings(Settings settings, Size<int> canvasSize) 
    => new ExpressionFactoryContextBuilder()
      .FromSeed(settings.Seed)
      .WithComplexity(settings.Complexity)
      .WithConstantRange(Range<int>.FromNegative(settings.ConstantRange))
      .WithCanvasSize(
        (canvasSize.As<float>() 
        * settings.RescaleFactor 
        + 1f).As<int>())
      .Normalize(settings.Normalized)
      .Build();
}