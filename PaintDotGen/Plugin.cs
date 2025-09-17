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
  private ExpressionContainer Expressions { get; } = new();

  protected override void OnPixelRender(IRenderingContext<ColorBgra32> context, Settings settings, Vector<int> position) 
    => context.Draw(position, settings.UseHsv 
      ? new ColorBgra32(ColorBgr24.Ceiling(Expressions.EvaluateToColorHsv96Float(position.As<float>() * settings.RescaleFactor).ToRgb()), 255)
      : Expressions.EvaluateToColorBgra32(position.As<float>() * settings.RescaleFactor));

  protected override void OnSettingsChanged(Settings oldSettings, Settings newSettings) {
    if (newSettings == oldSettings) return;
    var generatorContext = ExpressionFactoryContextBuilder
      .BuildFromSettings(newSettings, new Size<int>(Environment.Document.Size.Width, Environment.Document.Size.Height));
    Expressions.Regenerate(generatorContext);
  }

  protected override ControlInfo OnCreateConfigUI(PropertyCollection properties) 
    => this.GetDefaultConfigUI(properties);
  
  protected override PropertyCollection OnCreatePropertyCollection() 
    => this.GetPropertyCollection();
}