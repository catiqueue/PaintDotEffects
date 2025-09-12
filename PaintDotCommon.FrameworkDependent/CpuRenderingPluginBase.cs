using System.Drawing;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;

[PluginSupportInfo(typeof(CpuRenderingPluginBase<>))]
public abstract class CpuRenderingPluginBase<TSettings>(PluginInfoBase info) 
    // Paint.NET handles null image argument deep inside the call chain,
    // even though this concrete constructor doesn't think so
  : PropertyBasedBitmapEffect(info.DisplayName, info.Image!, info.SubMenu, BitmapEffectOptionsFactory.Create() with { IsConfigurable = true })
    , IPluginSupportInfoProvider 
  where TSettings : ISettings<TSettings> 
{
  private TSettings Settings { get; set; } = TSettings.Default;
  
  protected virtual void OnSettingsChanged(TSettings oldSettings, TSettings newSettings) { }
  protected abstract void OnRender(IRenderingContext<ColorBgra32> context, TSettings settings);

  protected sealed override void OnRender(IBitmapEffectOutput output) {
    using var source = Environment.GetSourceBitmapBgra32();
    using var sourceLock = source.Lock(source.Bounds());
    var sourceRegion = sourceLock.AsRegionPtr();
    
    var outputLocation = output.Bounds.Location;
    using var outputLock = output.LockBgra32();
    var outputRegion = outputLock.AsRegionPtr();
    
    OnRender(new CpuRenderingContext<ColorBgra32>(sourceRegion, outputRegion,  new Vector<int>(outputLocation.X, outputLocation.Y)), Settings);
  }

  protected sealed override void OnSetToken(PropertyBasedEffectConfigToken? newToken) {
    if(newToken is null) return;
    var newSettings = TSettings.FromConfigToken(newToken);
    OnSettingsChanged(Settings, newSettings);
    Settings = newSettings;
  }

  // help screen setup
  protected sealed override void OnCustomizeConfigUIWindowProperties(PropertyCollection props) {
    props[ControlInfoPropertyNames.WindowHelpContentType]!.Value = WindowHelpContentType.PlainText;
    props[ControlInfoPropertyNames.WindowHelpContent]!.Value = string.Format(info.Culture, "{0}\n{1} v{2}\n{3}\nAll rights reserved.", info.Copyright, info.DisplayName, info.VersionString, info.Author);
  }
  
  public IPluginSupportInfo GetPluginSupportInfo() => info;
}