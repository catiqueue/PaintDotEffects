using catiqueue.PaintDotNet.Plugins.Common;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal record Settings: ISettings<Settings> {
  public static Settings Default { get; } = new();
  
  public byte Rule { get; init; } = 30;
  public EcaBoundsHandlingMode BoundsHandler { get; init; } = EcaBoundsHandlingMode.ReturnOne;
  public bool RespectSourceColor { get; init; } = true;
  public EcaPointDescriptorPainter Painter { get; init; } = EcaPointDescriptorPainters.DefaultPalette;
  public EcaPointActivator Activator { get; init; } = EcaPointActivators.CreateTransparencyActivator(128);

  public static Settings FromConfigToken(PropertyBasedEffectConfigToken token)
    => token.ToSettings();
}