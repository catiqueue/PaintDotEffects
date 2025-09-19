using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Usability;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions;

internal delegate ColorBgra32 ExpressionInterpreter(ExpressionContainer expressions, Vector<float> input);

internal enum ExpressionInterpreterChoice { HSV, RGB, Grayscale }

internal static class ExpressionInterpreters {
  public static ExpressionInterpreter HSVInterpreter => (expressions, input) 
    => new ColorBgra32(ColorBgr24.Ceiling(expressions.EvaluateToColorHsv96Float(input).ToRgb()), 255);
  public static ExpressionInterpreter RGBInterpreter => (expressions, input)
    => expressions.EvaluateToColorBgra32(input);
  public static ExpressionInterpreter GrayscaleInterpreter => (expressions, input) 
    => new ColorGray8(expressions.A.EvaluateToByte(input.X, input.Y));

  public static ExpressionInterpreter FromChoice(ExpressionInterpreterChoice choice) => choice switch {
    ExpressionInterpreterChoice.HSV => HSVInterpreter,
    ExpressionInterpreterChoice.RGB => RGBInterpreter,
    ExpressionInterpreterChoice.Grayscale => GrayscaleInterpreter,
    _ => throw new ArgumentOutOfRangeException(nameof(choice), choice, null)
  };
}