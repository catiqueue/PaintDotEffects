using catiqueue.PaintDotNet.Plugins.Common;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen;

internal record Settings : ISettings<Settings> {
  public static Settings Default { get; } = new();
  
  public int Seed { get; init; } = 0;
  public int Complexity { get; init; } = 4;
  public bool Normalized { get; init; } = false;
  public int ConstantRange { get; init; } = 1024;
  public float RescaleFactor { get; init; } = 1f;
  public ExpressionInterpreter Interpreter { get; init; } = ExpressionInterpreters.HSVInterpreter;

  public static Settings FromConfigToken(PropertyBasedEffectConfigToken token) 
    => token.ToSettings();
};