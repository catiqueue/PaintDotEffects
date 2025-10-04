using System;
using System.Diagnostics;
using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using catiqueue.PaintDotNet.Plugins.Common.UI;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
// using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent.UI.Building;

namespace catiqueue.PaintDotNet.Plugins.Common;

public abstract class CpuRenderingUiPluginBase<TSettings> 
  : PropertyBasedBitmapEffect, IPluginSupportInfoProvider 
  where TSettings : class, ICloneable, new() 
{
  private readonly PluginInfoBase _info;
  private readonly TSettings _settings = new();
  private bool _settingsUpdateHappened = false;
  
  // apparently, paint.net destroys the plugin instance after you close the plugin window,
  // but preserves the PropertyCollection.
  // This makes it so that I can't regenerate the model from the constructor,
  // because I use static counters when creating property names.
  private static Lazy<PluginUiBehaviorModel<TSettings>>? _lazyUiModel;
  private static Lazy<ChangeTracker>? _lazyChangeTracker;
  
  private static ChangeTracker ChangeTracker => _lazyChangeTracker?.Value ?? throw new InvalidStateException("ChangeTracker not initialized");
  private static PluginUiBehaviorModel<TSettings> UiModel => _lazyUiModel?.Value ?? throw new InvalidStateException("UiModel not initialized");

  protected CpuRenderingUiPluginBase(PluginInfoBase info) : base(info.DisplayName, info.Image!, info.SubMenu, BitmapEffectOptionsFactory.Create() with { IsConfigurable = true }) {
    _info = info;
    _lazyUiModel ??= new(() => OnModelCreating(new PluginUiBehaviorBuilder<TSettings>()));
    _lazyChangeTracker ??= new(() => new(_lazyUiModel.Value.TriggeringProperties));
    Debug.WriteLine("Constructor");
  }
  
  protected abstract PluginUiBehaviorModel<TSettings> OnModelCreating(PluginUiBehaviorBuilder<TSettings> behaviorBuilder);
  protected virtual void OnRegenerationRequired(TSettings settings) { }

  protected virtual void OnRender(IRenderingContext<ColorBgra32> context, TSettings settings) {
    Debug.Write("OnRender");
    for (int y = context.DrawingArea.Top; y < context.DrawingArea.Bottom && !IsCancelRequested; y++) 
      for (int x = context.DrawingArea.Left; x < context.DrawingArea.Right; x++) 
        OnPixelRender(context, settings, new Vector<int>(x, y));
  }
  
  protected virtual void OnPixelRender(IRenderingContext<ColorBgra32> context, TSettings settings, Vector<int> position) { }

  protected sealed override void OnRender(IBitmapEffectOutput output) {
    using var source = Environment.GetSourceBitmapBgra32();
    using var sourceLock = source.Lock(source.Bounds());
    var sourceRegion = sourceLock.AsRegionPtr();
    
    var outputLocation = output.Bounds.Location;
    using var outputLock = output.LockBgra32();
    var outputRegion = outputLock.AsRegionPtr();
    
    OnRender(new CpuRenderingContext<ColorBgra32>(
      sourceRegion, 
      outputRegion, 
      new Vector<int>(outputLocation.X, outputLocation.Y)), 
      _settings.CloneT());
  }
  
  protected sealed override void OnSetToken(PropertyBasedEffectConfigToken? newToken) {
    Debug.WriteLine("OnSetToken");
    if(newToken is null) return;
    foreach (var binder in UiModel.Bindings)
      binder(_settings, newToken.Properties);
    ChangeTracker.Update(newToken.Properties);
    var regenerationRequired = ChangeTracker.Changes.Any();
    if (regenerationRequired || !_settingsUpdateHappened) 
      OnRegenerationRequired(_settings.CloneT());
    if(!_settingsUpdateHappened) _settingsUpdateHappened = true;
  }
  
  protected sealed override PropertyCollection OnCreatePropertyCollection() {
    Debug.WriteLine("OnCreatePropertyCollection");
    return new PropertyCollection(UiModel.Properties, UiModel.Rules);
  }

  protected sealed override ControlInfo OnCreateConfigUI(PropertyCollection props) {
    Debug.WriteLine("OnCreateConfigUI");
    return UiModel.GetControl(props);
  }

  // help screen setup
  protected sealed override void OnCustomizeConfigUIWindowProperties(PropertyCollection props)
    // let's see if the plaintext is default
    // props[ControlInfoPropertyNames.WindowHelpContentType]!.Value = WindowHelpContentType.PlainText;
    => props[ControlInfoPropertyNames.WindowHelpContent]!.Value = string.Format(
      _info.Culture, 
      "{0}\n{1} v{2}\n{3}\nAll rights reserved.", 
      _info.Copyright, _info.DisplayName, _info.VersionString, _info.Author);

  public IPluginSupportInfo GetPluginSupportInfo() => _info;
}