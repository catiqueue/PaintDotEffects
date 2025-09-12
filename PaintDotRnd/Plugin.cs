using System;
using System.Buffers;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using PaintDotNet.Rendering;
using static catiqueue.PaintDotNet.Plugins.Common.Math;

namespace catiqueue.PaintDotNet.Plugins.PaintDotRnd;

[PluginSupportInfo(typeof(PluginSupportInfo))]
internal sealed class Plugin() : PropertyBasedBitmapEffect(Info.DisplayName, Info.Icon, PluginSupportInfo.SubMenu, BitmapEffectOptionsFactory.Create() with { IsConfigurable = true }) {
  private static readonly PluginSupportInfo Info = new();
  
  private int InstanceSeed { get; } = (int) (DateTime.UtcNow.Ticks / 10000000 - 946684800);
  private Settings Settings { get; set; } = Settings.Default;
  private byte[] Cache { get; set; } = ArrayPool<byte>.Shared.Rent(2073600);
  private SizeInt32 DocumentSize => Environment.Document.Size;

  internal enum PropertyNames { Zoom, Precision, Seed }

  private void Render(RegionPtr<ColorBgra32> input, RegionPtr<ColorBgra32> output, Vector<int> offset) {
    Settings.Deconstruct(out var seed, out var precision, out var zoom);
    for(int y = 0; y < output.Height && !IsCancelRequested; y++) {
      for(int x = 0; x < output.Width; x++) {
        // i'm really tired rn so i got lost in this code so i had to break it up like this to find my way out of this
        byte grayscale = (byte) Precision(
                          value: Cache[Array2DAccessTo1D(
                            (new Vector<int>(x, y) + offset) / zoom, 
                            DocumentSize.Width)], 
                          maxValue: byte.MaxValue, 
                          precision);
        output[x, y] = (SrgbColorA) new ColorGray8(grayscale);
      }
    }
  }

  protected override void OnRender(IBitmapEffectOutput output) {
    using var source = Environment.GetSourceBitmapBgra32();
    using var sourceLock = source.Lock(output.Bounds);
    var sourceRegion = sourceLock.AsRegionPtr();
    
    var outputLocation = output.Bounds.Location;
    using var outputLock = output.LockBgra32();
    var outputRegion = outputLock.AsRegionPtr();

    Render(sourceRegion, outputRegion, new Vector<int>(outputLocation.X, outputLocation.Y));
  }

  protected override PropertyCollection OnCreatePropertyCollection() => new([
    new Int32Property(PropertyNames.Zoom, 1, 1, 128),
    // it makes sense to limit the precision to 256, but it doesn't make a noticeable difference
    // when this value is bigger than, like, 10, so i think it's fine to limit it to 128.
    new Int32Property(PropertyNames.Precision, 2, 2, 128),
    new Int32Property(PropertyNames.Seed, 0, 0, int.MaxValue)
  ], []);

  protected override void OnSetToken(PropertyBasedEffectConfigToken? newToken) {
    if (newToken?.ToSettings() is not { } newSettings) return;
    if (newSettings.Seed != Settings.Seed) {
      var rng = new Random(InstanceSeed ^ newSettings.Seed);
      if (DocumentSize.Area > Cache.Length) {
        ArrayPool<byte>.Shared.Return(Cache);
        Cache = ArrayPool<byte>.Shared.Rent((int) DocumentSize.Area);
      }
      rng.NextBytes(Cache);
    }
    
    Settings = newSettings;
  }

  protected override ControlInfo OnCreateConfigUI(PropertyCollection properties) {
    ControlInfo ConfigUI = CreateDefaultConfigUI(properties);

    ConfigUI.SetPropertyControlValue(PropertyNames.Zoom, ControlInfoPropertyNames.DisplayName, "Zoom");

    ConfigUI.SetPropertyControlValue(PropertyNames.Precision, ControlInfoPropertyNames.DisplayName, "Precision");

    ConfigUI.SetPropertyControlValue(PropertyNames.Seed, ControlInfoPropertyNames.DisplayName, string.Empty);
    ConfigUI.SetPropertyControlType(PropertyNames.Seed, PropertyControlType.IncrementButton);
    ConfigUI.SetPropertyControlValue(PropertyNames.Seed, ControlInfoPropertyNames.ButtonText, "Reseed");

    return ConfigUI;
  }

  protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection properties) {
    properties[ControlInfoPropertyNames.WindowHelpContentType]!.Value = WindowHelpContentType.PlainText;
    properties[ControlInfoPropertyNames.WindowHelpContent]!.Value = string.Format(PluginSupportInfo.Culture, "{0}\n{1} v{2}\n{3}\nAll rights reserved.", Info.Copyright, Info.DisplayName, Info.Version, Info.Author);

    base.OnCustomizeConfigUIWindowProperties(properties);
  }
}