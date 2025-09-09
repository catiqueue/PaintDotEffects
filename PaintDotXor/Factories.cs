using System;
using catiqueue.PaintDotNet.Plugins.PaintDotXor.Types;
using PaintDotNet.Imaging;
using Math = catiqueue.PaintDotNet.Plugins.Common.Math;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXor;

internal static class FilterFactory {
  public static Filter IsPrime => Math.IsPrime;
  public static Filter DivisibleBy(int divisor) => magic => Math.IsDivisible(magic, divisor);
}

internal static class PainterFactory {
  public static Painter SineHsvPainter => magic => ManagedColor.Create((SrgbColorA) new ColorHsv96Float(
    hue: MathF.Sin(magic) * 180f + 180f,
    saturation: 100f,
    value: MathF.Cos(magic) * 50f + 50f).ToRgb());
  public static Painter ConstantColorPainter(ManagedColor color) => _ => color;
}

internal static class OperationFactory {
  public static Operation XOR => pos => pos.X ^ pos.Y;
  public static Operation AND => pos => pos.X & pos.Y;
  public static Operation OR => pos => pos.X | pos.Y;

  public static Operation FromChoice(OperationChoice choice) => choice switch {
    OperationChoice.XOR => XOR,
    OperationChoice.AND => AND,
    OperationChoice.OR => OR,
    _ => throw new ArgumentOutOfRangeException(nameof(choice), "Unknown operation choice")
  };
}
