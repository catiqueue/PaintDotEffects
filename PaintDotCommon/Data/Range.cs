using System;
using System.Linq;
using System.Numerics;

namespace catiqueue.PaintDotNet.Plugins.Common.Data;

// starting to hate this
public readonly record struct Range<T>(T Start, T End, bool StartInclusive = true, bool EndInclusive = true) where T : INumber<T> {
  public bool IsBackwards => Start > End;
  public static Range<T> Empty { get; } = new(T.Zero, T.Zero);
  public static Range<T> Normalized { get; } = new(T.Zero, T.One);
  public static Range<T> Mathematic { get; } = new(-T.One, T.One);
  public static Range<T> FromSingleValue(T value) => new(value, value);
  public static Range<T> FromNegative(T value) => new(-value, value);
  
  public Range<TOther> As<TOther>() where TOther : INumber<TOther> =>
    new(TOther.CreateChecked(Start), TOther.CreateChecked(End), StartInclusive, EndInclusive);
  
  public bool Contains(T value) => (StartInclusive ? value >= Start : value > Start) && (EndInclusive ? value <= End : value < End);
  
  public Range<T> Abs() => Contains(T.Zero) 
    ? this with { Start = T.Zero, End = T.Max(T.Abs(Start), T.Abs(End)) }
    : this with { Start = T.Min(T.Abs(Start), T.Abs(End)), End = T.Max(T.Abs(Start), T.Abs(End)) };
  
  public T Min() => T.Min(Start, End);
  public T Max() => T.Max(Start, End);
  
  public static Range<T> operator -(Range<T> that) => that with { Start = -that.Start, End = -that.End };

  public static Range<T> operator +(Range<T> that, Range<T> other) {
    if (that.IsBackwards || other.IsBackwards)
      throw new NotSupportedException("Range mathematics is not supported for inverted ranges.");
    
    var minThat = T.Min(that.Start, that.End);
    var minOther = T.Min(other.Start, other.End);
    var maxThat = T.Max(that.Start, that.End);
    var maxOther = T.Max(other.Start, other.End);
    return new Range<T>(minThat + minOther, maxThat + maxOther, 
      that.StartInclusive || other.StartInclusive, that.EndInclusive || other.EndInclusive);
  }
  
  public static Range<T> operator -(Range<T> that, Range<T> other) {
    if (that.IsBackwards || other.IsBackwards)
      throw new NotSupportedException("Range mathematics is not supported for inverted ranges.");
    
    var minThat = T.Min(that.Start, that.End);
    var minOther = T.Min(other.Start, other.End);
    var maxThat = T.Max(that.Start, that.End);
    var maxOther = T.Max(other.Start, other.End);
    return new Range<T>(minThat - maxOther, maxThat - minOther, 
      that.StartInclusive || other.StartInclusive, that.EndInclusive || other.EndInclusive);
  }
  
  public static Range<T> operator *(Range<T> that, Range<T> other) {
    if (that.IsBackwards || other.IsBackwards)
      throw new NotSupportedException("Range mathematics is not supported for inverted ranges.");
    
    var minThat = T.Min(that.Start, that.End);
    var minOther = T.Min(other.Start, other.End);
    var maxThat = T.Max(that.Start, that.End);
    var maxOther = T.Max(other.Start, other.End);
    var corners = new[] { minThat * minOther, minThat * maxOther, maxThat * minOther, maxThat * maxOther };
    return new Range<T>(corners.Min()!, corners.Max()!, 
      that.StartInclusive || other.StartInclusive, that.EndInclusive || other.EndInclusive);
  }
  
  public static Range<T> operator /(Range<T> that, Range<T> other) {
    if (that.IsBackwards || other.IsBackwards)
      throw new NotSupportedException("Range mathematics is not supported for inverted ranges.");
    if (other.Start <= T.Zero || (other.EndInclusive ? other.End >= T.Zero : other.End < T.Zero))
      throw new DivideByZeroException($"If you want to divide by a value in the range {nameof(other)}, you will divide by zero at some point.");
    
    var minThat = T.Min(that.Start, that.End);
    var minOther = T.Min(other.Start, other.End);
    var maxThat = T.Max(that.Start, that.End);
    var maxOther = T.Max(other.Start, other.End);
    var corners = new[] { minThat / minOther, minThat / maxOther, maxThat / minOther, maxThat / maxOther };
    return new Range<T>(corners.Min()!, corners.Max()!, 
      that.StartInclusive || other.StartInclusive, that.EndInclusive || other.EndInclusive);
  }
  
  public override string ToString() => $"[{Start}, {End}]";
};