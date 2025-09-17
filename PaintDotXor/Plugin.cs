using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXor;

internal sealed class Plugin() : CpuRenderingPluginBase<Settings>(new PluginInfo()) {
  internal enum PropertyNames { OffsetX, OffsetY, Zoom, FilterMode, Divisor, Operation, UseHSV, Color }

  protected override void OnPixelRender(IRenderingContext<ColorBgra32> context, Settings settings, Vector<int> position) {
    settings.Deconstruct(out var operation, out var filter, out var painter, out var camera);
    int magic = operation(camera.ApplyTo(position));
    var result = filter(magic)
      ? painter(magic).GetBgra32(Environment.Document.ColorContext)
      : context.Read(position);
    context.Draw(position, result);
  }
  
  protected override PropertyCollection OnCreatePropertyCollection() => this.GetProperties();
  protected override ControlInfo OnCreateConfigUI(PropertyCollection properties) => this.GetConfigUI(properties);
}