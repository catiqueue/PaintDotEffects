using System;
using System.Buffers;
using catiqueue.PaintDotNet.Plugins.Common;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using catiqueue.PaintDotNet.Plugins.Common.UI;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Extensions;
using PaintDotNet.Imaging;
using static catiqueue.PaintDotNet.Plugins.Common.Math;

namespace catiqueue.PaintDotNet.Plugins.PaintDotRndTest;

// ReSharper disable once UnusedType.Global
internal sealed class Plugin() : CpuRenderingUiPluginBase<Settings>(new PluginInfo()) {
  private int InstanceSeed { get; } = (int) (DateTime.UtcNow.Ticks / 10000 - 946684800);
  private byte[] Cache { get; set; } = ArrayPool<byte>.Shared.Rent(2073600);

  protected override void OnPixelRender(IRenderingContext<ColorBgra32> context, Settings settings, Vector<int> position) {
    byte grayscale = (byte) Precision(
      value: Cache[Array2DAccessTo1D(position / settings.Zoom, context.RealArea.Size.Width)], 
      maxValue: byte.MaxValue, 
      precision: settings.Precision);
    context.Draw(position, new ColorGray8(grayscale));
  }

  protected override PluginUiBehaviorModel<Settings> OnModelCreating(PluginUiBehaviorBuilder<Settings> behaviorBuilder) =>
    behaviorBuilder.FromTabset()
      .WithTab("Seed")
        .WithIntegerSlider().BindTo(x => x.Seed).WithValueRange(new(int.MinValue, int.MaxValue)).ChangeTriggersRebuild().Then(out var seed)
        .WithCheckbox().Locks(seed).WhenUnchecked().Then().Then().Then()
      .WithTab("Visual")
        .WithIntegerSlider().BindTo(x => x.Zoom).WithValueRange(new(1, 128)).Then()
        .WithChoiceList<int>().BindTo(x => x.Precision).WithChoice(2, "Binary").WithChoice(3, "3 Colors").WithChoice(4, "4 Colors").WithChoice(byte.MaxValue, "Full Spectrum").End()
      .Build();

  protected override void OnRegenerationRequired(Settings settings) {
    var rng = new Random(InstanceSeed ^ settings.Seed);
    if (Environment.Document.Size.Area > Cache.Length) {
      ArrayPool<byte>.Shared.Return(Cache);
      Cache = ArrayPool<byte>.Shared.Rent((int) Environment.Document.Size.Area);
    }
    rng.NextBytes(Cache);
  }
}