using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal sealed class Plugin() : CpuRenderingPluginBase<Settings>(new PluginInfo()) {
  internal enum PropertyNames { MaxWidth, Generations, Rule, BoundsHandling, Zoom, UseRtC, Color }

  private EcaMachine _eca = null!;

  protected override void OnPixelRender(IRenderingContext<ColorBgra32> context, Settings settings, Vector<int> position) {
    _eca.Read(position).Deconstruct(out var descriptor, out var isActive);
    context.Draw(position, isActive 
      ? descriptor == EcaPointDescriptor.Pregenerated && settings.RespectSourceColor 
        ? context.Read(position) 
        : settings.Painter(descriptor) 
      : ColorBgra32.FromUInt32(uint.MinValue));
  }

  protected override void OnSettingsChanged(Settings oldSettings, Settings newSettings, bool firstChange) {
    if (oldSettings.Rule == newSettings.Rule && oldSettings.BoundsHandler == newSettings.BoundsHandler && !firstChange)
      return;
    
    using var source = Environment.GetSourceBitmapBgra32();
    using var sourceLock = source.Lock(source.Bounds());
    var sourceRegion = sourceLock.AsRegionPtr();
    var wrapper = new RegionPtrWrapper<ColorBgra32>(sourceRegion, Vector<int>.Zero);
    var converter = new EcaConvertingCanvasReader(wrapper, newSettings.Activator);
    
    _eca = new EcaMachine(converter) {
      Rule = newSettings.Rule,
      BoundsHandler = newSettings.BoundsHandler
    };
  }

  protected override PropertyCollection OnCreatePropertyCollection()
    => this.GetPropertyCollection();
  
  protected override ControlInfo OnCreateConfigUI(PropertyCollection properties)
    => this.GetConfigUI(properties);

}