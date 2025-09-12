using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

internal static class Math {
  public static bool IsLineActive(int num, int spacer) => num % spacer == 0;
  private static bool IsLineEven(int num, int spacer) => num % (spacer * 2) == 0;
  // I can't remember why I'm computing this like that
  public static bool IsLineEvenCombined(Vector<int> pos, Vector<int> spacer) => IsLineEven(pos.X, spacer.X) ^ IsLineEven(pos.Y, spacer.Y);
  public static bool Filter(Vector<int> pos, Vector<int> spacer) => IsLineActive(pos.X, spacer.X) || IsLineActive(pos.Y, spacer.Y);
  public static bool IsJunction(Vector<int> pos, Vector<int> spacer) => IsLineActive(pos.X, spacer.X) && IsLineActive(pos.Y, spacer.Y);
}