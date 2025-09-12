using System;
using static catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production.Storage;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production;

internal static class ExpressionFactory {
  internal delegate IGeneratorExpression ExpressionFactoryDelegate(ExpressionFactoryContext context);
  
  internal static IGeneratorExpression CreateExpression(ExpressionFactoryContext context) {
    var rng = new Random(context.Seed);
    var shouldEnd = rng.NextDouble() <= context.EndingChance;
    return shouldEnd ? ConstantFactories[rng.Next(ConstantFactories.Length)](context)
                     : FunctionFactories[rng.Next(FunctionFactories.Length)](context);
  }
}