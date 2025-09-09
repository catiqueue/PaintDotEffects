namespace catiqueue.PaintDotNet.Plugins.Common;


public enum BoundsHandlingMode { None = 0, Clamp, Wrap, Mirror }

public static class BoundsHandling {
  public static int HandleFor(int length, int index, BoundsHandlingMode mode) => mode switch {
    BoundsHandlingMode.Clamp => Clamp(length, index),
    BoundsHandlingMode.Wrap => Wrap(length, index),
    BoundsHandlingMode.Mirror => Mirror(length, index),
    _ => index 
  };
  
  public static int Clamp(int length, int index) 
    => System.Math.Clamp(index, 0, length - 1);
  
  public static int Wrap(int length, int index) 
    => (index % length + length) % length;
  
  public static int Mirror(int length, int index) {
    int period = length * 2;
    // basically index % length but wraps negatives as well;
    int mod = Wrap(period, index);
    return mod < length ? mod : FlipAroundCenter(period, mod);
  }
  
  public static int FlipAroundCenter(int length, int value) 
    => length - 1 - value;
}