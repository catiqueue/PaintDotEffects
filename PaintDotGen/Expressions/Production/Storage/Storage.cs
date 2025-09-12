using System;
using System.Collections.Immutable;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Basic;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Trigonometry;
using static catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production.ExpressionFactory;


namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production;

internal static class Storage {
  internal static readonly ImmutableArray<ExpressionFactoryDelegate> FunctionFactories = [
    (context) => new AdditionGeneratorExpression(
      left: CreateExpression(context.LevelDown),
      right: CreateExpression(context.LevelDown)),
    (context) => new SubtractionGeneratorExpression(
      left: CreateExpression(context.LevelDown),
      right: CreateExpression(context.LevelDown)),
    
    (context) => new AbsGeneratorExpression(CreateExpression(context.LevelDown)),
    
    (context) => new MinGeneratorExpression(
      arg1: CreateExpression(context.LevelDown), 
      arg2: CreateExpression(context.LevelDown)),
    (context) => new MaxGeneratorExpression(
      arg1: CreateExpression(context.LevelDown), 
      arg2: CreateExpression(context.LevelDown)),
    
    (context) => new SinGeneratorExpression(CreateExpression(context.LevelDown)),
    (context) => new CosGeneratorExpression(CreateExpression(context.LevelDown)),
    
    (context) => new TanhGeneratorExpression(CreateExpression(context.LevelDown)),
    (context) => new AtanGeneratorExpression(CreateExpression(context.LevelDown)),
  ];

  internal static readonly ImmutableArray<ExpressionFactoryDelegate> ConstantFactories = [
    (ctx) => new ConstantGeneratorExpression(new Random(ctx.Seed).Next(
        ctx.Options.ConstantRange.Start,
        ctx.Options.ConstantRange.End),
      ctx.Options.ConstantRange.As<float>()),
    (ctx) => ctx.Parameters.X,
    (ctx) => ctx.Parameters.Y
  ];
}