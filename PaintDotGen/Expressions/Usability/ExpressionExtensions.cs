using catiqueue.PaintDotNet.Plugins.Common;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Usability;

internal static class ExpressionExtensions {
  public static float EvaluateNormalized(this IGeneratorExpression expr, float x, float y)
    => Math.Normalize(expr.Evaluate(x, y), expr.ValueRange);
  
  public static byte EvaluateToByte(this IGeneratorExpression expr, float x, float y) 
    => (byte) (Math.Normalize(expr.Evaluate(x, y), expr.ValueRange) * byte.MaxValue);
  
  public static ColorBgra32 EvaluateToColorBgra32(this ExpressionContainer container, float x, float y)
  => new(b: container.B.EvaluateToByte(x, y), 
         g: container.G.EvaluateToByte(x, y), 
         r: container.R.EvaluateToByte(x, y), 
         a: 255);
  
  public static ColorBgra32 EvaluateToColorBgra32(this ExpressionContainer container, Vector<float> pos)
    => container.EvaluateToColorBgra32(pos.X, pos.Y);

  public static ColorHsv96Float EvaluateToColorHsv96Float(this ExpressionContainer container, float x, float y)
    => new(hue:        container.H.EvaluateNormalized(x, y) * ColorHsv96Float.HueMaxValue,
           saturation: container.S.EvaluateNormalized(x, y) * ColorHsv96Float.SaturationMaxValue,
           value:      container.V.EvaluateNormalized(x, y) * ColorHsv96Float.ValueMaxValue);

  public static ColorHsv96Float EvaluateToColorHsv96Float(this ExpressionContainer container, Vector<float> pos)
    => container.EvaluateToColorHsv96Float(pos.X, pos.Y);
}