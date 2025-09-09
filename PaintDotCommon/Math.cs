using System;
using static System.Math;

namespace catiqueue.PaintDotNet.Plugins.Common;

public static class Math {
  public static float Scale(float value, float minValue, float maxValue, float minLimit, float maxLimit) 
    => IsInRange(value, minValue, maxValue) 
      ? (maxLimit - minLimit) * (value - minValue) / (maxValue - minValue) + minLimit 
      : throw new ArgumentException($"The value {value} is not in range {minValue}-{maxValue} or the range is invalid.", nameof(value));

  // before, there were checks for NaN and infinities, but this shouldn't be possible.
  public static float Normalize(float value, float minValue, float maxValue) 
    => Scale(value, minValue, maxValue, 0, 1);

  public static int Round(float value) => (int) CopySign(Abs(value) + .5f, value);
  
  // what even is this? \/ 
  public static int Precision(int value, int maxValue, int precision) => (int) (Round(value / ((float)maxValue / (precision - 1))) * ((float)maxValue / (precision - 1)));
  
  public static bool IsInRange(float value, float minValue, float maxValue) => minValue <= value && value <= maxValue;
  
  public static bool IsPrime(int num) {
    if (num <= 1 || (num % 2 == 0 && num > 2)) return false;
    for (int i = 3; i < Floor(Sqrt(num)); i += 2)
      if (num % i == 0)
        return false;
    return true;
  }
  
  public static bool IsDivisible(int num, int divisor) => num % divisor == 0;
}
