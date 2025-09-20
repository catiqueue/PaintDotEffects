using System;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal enum EcaPainterChoice { Default, }

internal delegate ColorBgra32 EcaPointDescriptorPainter(EcaPointDescriptor descriptor);

internal static class EcaPointDescriptorPainters {
  public static EcaPointDescriptorPainter DefaultPalette => descriptor => descriptor switch {
    EcaPointDescriptor.None => (SrgbColor)new ColorHsv96Float(360f / 10f * 1, 100f, 100f).ToRgb(),
    EcaPointDescriptor.Pregenerated => (SrgbColor)new ColorHsv96Float(360f / 10f * 2, 100f, 100f).ToRgb(),
    EcaPointDescriptor.Zero => (SrgbColor)new ColorHsv96Float(360f / 10f * 3, 100f, 100f).ToRgb(),
    EcaPointDescriptor.One => (SrgbColor)new ColorHsv96Float(360f / 10f * 4, 100f, 100f).ToRgb(),
    EcaPointDescriptor.Two => (SrgbColor)new ColorHsv96Float(360f / 10f * 5, 100f, 100f).ToRgb(),
    EcaPointDescriptor.Three => (SrgbColor)new ColorHsv96Float(360f / 10f * 6, 100f, 100f).ToRgb(),
    EcaPointDescriptor.Four => (SrgbColor)new ColorHsv96Float(360f / 10f * 7, 100f, 100f).ToRgb(),
    EcaPointDescriptor.Five => (SrgbColor)new ColorHsv96Float(360f / 10f * 8, 100f, 100f).ToRgb(),
    EcaPointDescriptor.Six => (SrgbColor)new ColorHsv96Float(360f / 10f * 9, 100f, 100f).ToRgb(),
    EcaPointDescriptor.Seven => (SrgbColor)new ColorHsv96Float(360f, 100f, 100f).ToRgb(),
    _ => new ColorBgra32(0U)
  };

  public static EcaPointDescriptorPainter FromChoice(EcaPainterChoice choice) => choice switch {
    EcaPainterChoice.Default => DefaultPalette,
    _ => throw new ArgumentOutOfRangeException(nameof(choice), choice, null)
  };
}