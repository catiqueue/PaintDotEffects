using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common;

[Obsolete("This will be replaced")]
[PluginSupportInfo(typeof(CpuRenderingPluginBase<>))]
public abstract class CpuRenderingPluginBase<TSettings>(PluginInfoBase info) 
    // Paint.NET handles null image argument deep inside the call chain,
    // even though this concrete constructor doesn't think so
  : PropertyBasedBitmapEffect(info.DisplayName, info.Image!, info.SubMenu, BitmapEffectOptionsFactory.Create() with { IsConfigurable = true })
  , IPluginSupportInfoProvider 
  where TSettings : class, ISettings<TSettings> {
  private TSettings _settings = TSettings.Default;
  private bool _settingsUpdateHappened = false; 

  protected virtual void OnRender(IRenderingContext<ColorBgra32> context, TSettings settings) {
    for (int y = context.DrawingArea.Top; y < context.DrawingArea.Bottom && !IsCancelRequested; y++) {
      for (int x = context.DrawingArea.Left; x < context.DrawingArea.Right; x++) {
        OnPixelRender(context, settings, new Vector<int>(x, y));
      }
    }
  }
  protected virtual void OnPixelRender(IRenderingContext<ColorBgra32> context, TSettings settings, Vector<int> position) { }

  protected sealed override void OnRender(IBitmapEffectOutput output) {
    using var source = Environment.GetSourceBitmapBgra32();
    using var sourceLock = source.Lock(source.Bounds());
    var sourceRegion = sourceLock.AsRegionPtr();
    
    var outputLocation = output.Bounds.Location;
    using var outputLock = output.LockBgra32();
    var outputRegion = outputLock.AsRegionPtr();
    
    OnRender(new CpuRenderingContext<ColorBgra32>(sourceRegion, outputRegion,  new Vector<int>(outputLocation.X, outputLocation.Y)), _settings);
  }
  
  protected virtual void OnSettingsChanged(TSettings oldSettings, TSettings newSettings, bool firstChange) { }
  
  protected sealed override void OnSetToken(PropertyBasedEffectConfigToken? newToken) {
    if(newToken is null) return;
    var newSettings = TSettings.FromConfigToken(newToken);
    OnSettingsChanged(_settings, newSettings, !_settingsUpdateHappened);
    _settings = newSettings;
    if(!_settingsUpdateHappened) _settingsUpdateHappened = true;
  }

  // help screen setup
  protected sealed override void OnCustomizeConfigUIWindowProperties(PropertyCollection props) {
    props[ControlInfoPropertyNames.WindowHelpContentType]!.Value = WindowHelpContentType.PlainText;
    props[ControlInfoPropertyNames.WindowHelpContent]!.Value = string.Format(info.Culture, "{0}\n{1} v{2}\n{3}\nAll rights reserved.", info.Copyright, info.DisplayName, info.VersionString, info.Author);
  }
  
  public IPluginSupportInfo GetPluginSupportInfo() => info;
}