using System;
using catiqueue.PaintDotNet.Plugins.Common;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using catiqueue.PaintDotNet.Plugins.Common.UI;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Extensions;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXorTest;

internal sealed class Plugin() : CpuRenderingUiPluginBase<Settings>(new PluginInfo()) {
  protected override PluginUiBehaviorModel<Settings> OnModelCreating(PluginUiBehaviorBuilder<Settings> behaviorBuilder) {
    return behaviorBuilder.FromPanel()
      .WithIntegerSlider("X offset").WithValueRange(new(0, short.MaxValue)).Then(out var xOffset)
      .WithIntegerSlider("Y offset").WithValueRange(new(0, short.MaxValue)).Then(out var yOffset)
      .WithIntegerSlider("Zoom").WithValueRange(new(1, 128)).Then(out var zoom)
      .WithChoiceList<FilterChoice>("Filter mode")
        .WithChoice(FilterChoice.IsPrime, "Is prime", out var primeChoice)
        .WithChoice(FilterChoice.IsDivisible, "Is divisible", out var divisibleChoice).Then(out var filterMode)
      .WithIntegerSlider("Divisor").WithValueRange(new(3, 512)).LockedBy(filterMode).WhenNotAnyOf(divisibleChoice).Then().Then(out var divisor)
      .WithChoiceList<OperationChoice>("Operation").BindTo(x => x.Operation, OperationFactory.FromChoice)
        .WithChoice(OperationChoice.XOR)
        .WithChoice(OperationChoice.AND)
        .WithChoice(OperationChoice.OR)
        .WithChoice(OperationChoice.BitReversedXOR, "Bit reversed XOR")
        .WithChoice(OperationChoice.GrayCodeXOR, "Gray encoded XOR").Then()
      .WithCheckbox("Use HSV").Then(out var useHsv)
      .WithColorPicker("Pattern color").LockedBy(useHsv).Then().Then(out var color).End()
      
      .WithBinding(xOffset, yOffset, zoom, settings => settings.Camera, (x, y, z) => new Camera(new Vector<int>(x,y), z))
      .WithBinding(filterMode, divisor, settings => settings.Filter, (filter, divisor) => filter switch {
        FilterChoice.IsPrime => FilterFactory.IsPrime,
        FilterChoice.IsDivisible => FilterFactory.DivisibleBy(divisor)
      })
      .WithBinding(useHsv, color, settings => settings.Painter, (useHsv, color) => useHsv ? PainterFactory.SineHsvPainter : PainterFactory.ConstantColorPainter(color))
      .Build();
  }

  protected override void OnPixelRender(IRenderingContext<ColorBgra32> context, Settings settings, Vector<int> position) {
    var (operation, filter, painter, camera) = settings;
    int magic = operation(camera.ApplyTo(position));
    var result = filter(magic)
      ? painter(magic).GetBgra32(Environment.Document.ColorContext)
      : context.Read(position);
    context.Draw(position, result);
  }
}