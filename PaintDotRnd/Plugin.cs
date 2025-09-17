using System;
using System.Buffers;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using static catiqueue.PaintDotNet.Plugins.Common.Math;

namespace catiqueue.PaintDotNet.Plugins.PaintDotRnd;


internal sealed class Plugin() : CpuRenderingPluginBase<Settings>(new PluginInfo()) {
  internal enum PropertyNames { Zoom, Precision, Seed }
  
  private int InstanceSeed { get; } = (int) (DateTime.UtcNow.Ticks / 10000000 - 946684800);
  private byte[] Cache { get; set; } = ArrayPool<byte>.Shared.Rent(2073600);

  protected override void OnPixelRender(IRenderingContext<ColorBgra32> context, Settings settings, Vector<int> position) {
    byte grayscale = (byte) Precision(
      value: Cache[Array2DAccessTo1D(position / settings.Zoom, context.RealArea.Size.Width)], 
      maxValue: byte.MaxValue, 
      precision: settings.Precision);
    context.Draw(position, new ColorGray8(grayscale));
  }

  protected override void OnSettingsChanged(Settings oldSettings, Settings newSettings) {
    if (newSettings.Seed == oldSettings.Seed) return;
    
    var rng = new Random(InstanceSeed ^ newSettings.Seed);
    if (Environment.Document.Size.Area > Cache.Length) {
      ArrayPool<byte>.Shared.Return(Cache);
      Cache = ArrayPool<byte>.Shared.Rent((int) Environment.Document.Size.Area);
    }
    
    rng.NextBytes(Cache);
  }
  
  protected override PropertyCollection OnCreatePropertyCollection()
    => this.GetPropertyCollection();

  protected override ControlInfo OnCreateConfigUI(PropertyCollection properties) 
    => this.GetConfigUI(properties);
}