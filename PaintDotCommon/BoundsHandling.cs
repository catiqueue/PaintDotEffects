using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.Common;


public enum BoundsHandlingMode { Clamp, Wrap, Mirror }

public static class BoundsHandling {
  
  public static Vector<int> HandleFor(Vector<int> pos, Size<int> bounds, BoundsHandlingMode mode)
    => new(HandleFor(bounds.Width, pos.X, mode), HandleFor(bounds.Height, pos.Y, mode));
  
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
  
  public static int FlipAroundCenter(int length, int index) 
    => length - 1 - index;
}