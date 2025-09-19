using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal delegate bool EcaPointActivator(ColorBgra32 color);

internal static class EcaPointActivators {
  public static EcaPointActivator CreateGrayscaleActivator(byte threshold) => color
    => (color.R + color.G + color.B) / 3 > threshold;

  public static EcaPointActivator CreateTransparencyActivator(byte threshold) => color
    => color.A > threshold;
}