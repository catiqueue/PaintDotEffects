using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Binding;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;
using PropertyConfigEntry = System.Collections.Generic.KeyValuePair<PaintDotNet.IndirectUI.ControlInfoPropertyNames, object>;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public abstract class UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue>(PluginUiBehaviorBuilder<TSettings> root, TParent parent)
  : UiBuilderBase<TSettings, TParent, UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue>, TResult>(root, parent)
  where TSelf : UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue>
  where TResult : ValueNodeBase</* TSettings, */ TValue>
  where TSettings : class 
  {
  // even though these properties are applicable to all controls (theoretically),
  // I haven't encountered a case where I would need them outside a value control.
  protected Dictionary<ControlInfoPropertyNames, object> Configuration { get; } = new();
  protected PropertyControlType ControlType { get; set; }
  
  protected TValue? DefaultValue { get; private set; }
  // protected Expression<Func</* TSettings, */ TResultType>>? Selector { get; private set; }
  // protected bool TriggersRebuild { get; private set; }

  public new TSelf WithName(string name) => (TSelf) base.WithName(name);

  // you can't check the nullability of this value in inherited classes so this workaround is necessary
  protected bool DefaultSet { get; private set; }
  public TSelf WithDefault(TValue defaultValue) {
    DefaultValue = defaultValue;
    DefaultSet = true;
    return (TSelf) this;
  }
  
  protected void SetDisplayName(string name) => Configuration[ControlInfoPropertyNames.DisplayName] = name;
  
  /* public TSelf ChangeTriggersRebuild() {
    TriggersRebuild = true;
    return (TSelf) this;
  } */
  
  /* public TSelf BindTo(Expression<Func<TSettings, TResultType>> selector) {
    selector.ValidateSelector(out _);
    Selector = selector;
    return (TSelf) this;
  } */
}