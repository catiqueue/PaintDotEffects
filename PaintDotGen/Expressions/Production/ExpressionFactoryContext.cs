namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production;

internal sealed class ExpressionFactoryContext(ExpressionFactoryOptions options, ExpressionFactoryParameters parameters) {
  public ExpressionFactoryOptions Options => options;
  public ExpressionFactoryParameters Parameters => parameters;
  
  public ExpressionFactoryContext LevelDown => new(options with { Level = options.Level - 1 }, parameters);
  
  public int Seed => options.Seed ^ options.Complexity ^ options.Level;
  public float EndingChance => (options.Complexity - options.Level) / (options.Complexity - 1f);
}