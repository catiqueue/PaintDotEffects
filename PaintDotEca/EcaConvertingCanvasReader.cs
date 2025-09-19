using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal class EcaConvertingCanvasReader(IReadonlyCanvas<ColorBgra32> source, EcaPointActivator activator) 
    : ReadonlyCanvasConverterBase<ColorBgra32, bool>(source) {
  protected override bool Convert(ColorBgra32 value) => activator(value);
}
