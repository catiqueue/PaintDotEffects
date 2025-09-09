using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using static System.Math;

namespace catiqueue.PaintDotNet.Plugins.Common;

public static class Math {
  /// <summary>
  /// Transforms the value with the known range valueRange to the range transformRange.
  /// </summary>
  /// <param name="value">The value to be transformed</param>
  /// <param name="valueRange">The known value range</param>
  /// <param name="transformRange">The range to transform the value to</param>
  /// <returns>The transformed value</returns>
  /// <exception cref="ArgumentException">The exception is thrown when the value is not in range valueRange or the range is going negative direction</exception>
  public static float Scale(float value, Range<float> valueRange, Range<float> transformRange) 
    => valueRange.Contains(value)
      ? (transformRange.End - transformRange.Start) 
        * (value - valueRange.Start) / (valueRange.End - valueRange.Start) 
        + transformRange.Start 
      : throw new ArgumentException($"The value {value} is not in range {valueRange.Start}-{valueRange.End} or the range is invalid.", nameof(value));

  /// <summary>
  /// Normalizes the value with the known range valueRange to the normalized range 0..1.
  /// </summary>
  /// <param name="value">The value to be normalized</param>
  /// <param name="valueRange">The known value range</param>
  /// <returns>The normalized value</returns>
  public static float Normalize(float value, Range<float> valueRange) 
    => Scale(value, valueRange, Range<float>.Normalized);

  /// <summary>
  /// Rounds the value to the nearest integer from 0.5.
  /// </summary>
  /// <param name="value">The value to be rounded</param>
  /// <returns>The nearest integer</returns>
  public static int Round(float value) 
    => (int) CopySign(Abs(value) + .5f, value);
  
  /// <summary>
  /// This function divides the range 0..maxValue to {precision} sectors and rounds the value to the nearest sector!
  /// </summary>
  /// <param name="value">The value to round</param>
  /// <param name="maxValue">The maximum value to round the value to</param>
  /// <param name="precision">The number of sectors</param>
  /// <returns>The rounded value</returns>
  public static int Precision(int value, int maxValue, int precision) {
    float step = maxValue / (precision - 1f);
    return (int)(Round(value / step) * step);
  }

  public static bool IsPrime(int num) {
    if (num <= 1 || (num % 2 == 0 && num > 2)) return false;
    for (int i = 3; i < Floor(Sqrt(num)); i += 2)
      if (num % i == 0)
        return false;
    return true;
  }
  
  public static bool IsDivisible(int num, int divisor) 
    => num % divisor == 0;
  
  /// <summary>
  /// Simplifies access to one-dimensional arrays, which are used as a storage for two-dimensional data.
  /// </summary>
  /// <param name="pos">The position to access</param>
  /// <param name="width">The width of the imagined two-dimensional array</param>
  /// <returns>The index into the one-dimensional array</returns>
  public static int Array2DAccessTo1D(Vector2I pos, int width) 
    => pos.Y * width + pos.X;
}
