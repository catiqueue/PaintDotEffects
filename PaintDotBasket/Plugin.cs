using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using static catiqueue.PaintDotNet.Plugins.PaintDotBasket.Math;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

internal sealed class Plugin() : CpuRenderingPluginBase<Settings>(new PluginInfo()) {
  internal enum PropertyNames { BackgroundColor, FirstColor, SecondColor, XSpacer, YSpacer, XSize, YSize }

  protected override void OnPixelRender(IRenderingContext<ColorBgra32> context, Settings settings, Vector<int> position) {
    context.Draw(position, Filter(position / settings.Size, settings.Spacer + Vector<int>.One)
      ? IsJunction(position / settings.Size, settings.Spacer + Vector<int>.One)
        ? IsLineEvenCombined(position / settings.Size, settings.Spacer + Vector<int>.One)
          ? settings.FirstColor.GetSrgb()
          : settings.SecondColor.GetSrgb()
        : IsLineActive((position.Y) / settings.Size.Y, settings.Spacer.Y + 1)
          ? settings.SecondColor.GetSrgb()
          : settings.FirstColor.GetSrgb()
      : settings.BackgroundColor.GetSrgb());
  }

  protected override PropertyCollection OnCreatePropertyCollection() 
    => this.GetPropertyCollection();

  protected override ControlInfo OnCreateConfigUI(PropertyCollection properties) 
    => this.GetConfigUI(properties);
}