using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Production;
using catiqueue.PaintDotNet.Plugins.PaintDotGen.Expressions.Usability;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen;

internal enum PropertyNames { Seed, Complexity, ConstantRange, ImaginedCanvasRescaleFactor, Normalized, UseHsv }

internal sealed class Plugin() : CpuRenderingPluginBase<Settings>(new PluginInfo()) {
  private Random Rng { get; set; } = new();
  private ExpressionContainer Expressions { get; } = new();

  protected override void OnRender(IRenderingContext<ColorBgra32> context, Settings settings) {
    for (int y = context.DrawingArea.Top; y < context.DrawingArea.Bottom && !IsCancelRequested; y++) {
      for (int x = context.DrawingArea.Left; x < context.DrawingArea.Right; x++) {
        var pos = new Vector<int>(x, y);
        context.Draw(pos, settings.UseHsv 
          ? new ColorBgra32(ColorBgr24.Ceiling(Expressions.EvaluateToColorHsv96Float(pos.As<float>() * settings.RescaleFactor).ToRgb()), 255)
          : Expressions.EvaluateToColorBgra32(pos.As<float>() * settings.RescaleFactor));
      }
    }
  }
  
  protected override void OnSettingsChanged(Settings oldSettings, Settings newSettings) {
    if (newSettings == oldSettings) return;
    var generatorContext = new ExpressionFactoryContextBuilder()
      .FromSeed(newSettings.Seed)
      .WithComplexity(newSettings.Complexity)
      // ConstantRange doesn't work for some reason. (?)
      .WithConstantRange(Range<int>.FromNegative(newSettings.ConstantRange))
      .WithCanvasSize(
        (new Size<int>(Environment.Document.Size.Width, Environment.Document.Size.Height)
        .As<float>() 
        * newSettings.RescaleFactor 
        + 1f)
        .As<int>())
      .Normalize(newSettings.Normalized)
      .Build();
    Expressions.Regenerate(generatorContext);
  }

  protected override ControlInfo OnCreateConfigUI(PropertyCollection properties) 
    => this.GetDefaultConfigUI(properties);
  
  protected override PropertyCollection OnCreatePropertyCollection() 
    => this.GetPropertyCollection();
}