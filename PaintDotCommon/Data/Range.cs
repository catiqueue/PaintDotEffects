using System.Numerics;

namespace catiqueue.PaintDotNet.Plugins.Common.Data;

public readonly record struct Range<T>(T Start, T End, bool EndInclusive = true) where T : INumber<T> {
  public static Range<T> Empty => new(T.Zero, T.Zero);
  public static Range<T> Normalized => new(T.Zero, T.One);
  public bool Contains(T value) => Start <= value && (EndInclusive ? value <= End : value < End);
  public bool IsBackwards => Start > End;
};