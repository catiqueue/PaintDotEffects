using System;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal enum EcaActivatorChoice { Grayscale, Transparency, Intensity }

internal delegate bool EcaPointActivator(ColorBgra32 color);

internal static class EcaPointActivators {
  public static EcaPointActivator CreateGrayscaleActivator(byte threshold) => color
    => (color.R + color.G + color.B) / 3 > threshold;

  public static EcaPointActivator CreateTransparencyActivator(byte threshold) => color
    => color.A > threshold;
  
  public static EcaPointActivator CreateIntensityActivator(byte threshold) => color
    => color.Intensity > threshold;
  
  public static EcaPointActivator FromChoice(EcaActivatorChoice choice, byte threshold) => choice switch {
    EcaActivatorChoice.Grayscale => CreateGrayscaleActivator(threshold),
    EcaActivatorChoice.Transparency => CreateTransparencyActivator(threshold),
    EcaActivatorChoice.Intensity => CreateIntensityActivator(threshold),
    _ => throw new ArgumentOutOfRangeException(nameof(choice), choice, null)
  };
}