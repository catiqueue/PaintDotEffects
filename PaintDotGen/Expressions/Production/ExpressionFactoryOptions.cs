using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production;

internal sealed class ExpressionFactoryOptions {
  public int Seed { get; set; }
  public int Complexity { get; set; }
  public int Level { get; set; }
  public Range<int> ConstantRange { get; set; }
};