using System.Collections.Generic;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;

public abstract class UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue>(PluginUiBehaviorBuilder<TSettings> root, TParent parent)
  : UiBuilderBase<TSettings, TParent, UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue>, TResult>(root, parent)
  where TSelf : UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue>
  where TResult : ValueNodeBase<TValue>
  where TSettings : class 
  {
  // even though these properties are applicable to all controls (theoretically),
  // I haven't encountered a case where I would need them outside a value control.
  protected Dictionary<ControlInfoPropertyNames, object> Configuration { get; } = new();
  protected PropertyControlType ControlType { get; set; }
  
  protected TValue? DefaultValue { get; private set; }

  public new TSelf WithName(string name) => (TSelf) base.WithName(name);

  // you can't check the nullability of this value in inherited classes so this workaround is necessary
  protected bool DefaultSet { get; private set; }
  public TSelf WithDefault(TValue defaultValue) {
    DefaultValue = defaultValue;
    DefaultSet = true;
    return (TSelf) this;
  }
  
  protected void SetDisplayName(string name) => Configuration[ControlInfoPropertyNames.DisplayName] = name;
}