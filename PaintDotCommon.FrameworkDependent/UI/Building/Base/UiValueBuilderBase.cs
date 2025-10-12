using System.Collections.Generic;
using System.Linq;
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
  private readonly Dictionary<ControlInfoPropertyNames, object> _configuration = new();
  
  protected PropertyControlType ControlType { get; set; }
  protected IEnumerable<PropertyConfigEntry> Configuration => _configuration.Select(kvp => new PropertyConfigEntry(kvp.Key, kvp.Value));
  
  protected TValue? DefaultValue { get; private set; }

  public new TSelf WithName(string name) => (TSelf) base.WithName(name);
  protected override void OnSetName(string name) => WithDisplayName(name);

  // you can't check the nullability of this value in inherited classes so this workaround is necessary
  protected bool DefaultSet { get; private set; }
  public TSelf WithDefault(TValue defaultValue) {
    DefaultValue = defaultValue;
    DefaultSet = true;
    return (TSelf) this;
  }
  
  protected TSelf WithConfiguration(ControlInfoPropertyNames name, object value) {
    _configuration[name] = value;
    return (TSelf) this;
  }

  protected TSelf WithDisplayName(string name) => WithConfiguration(ControlInfoPropertyNames.DisplayName, name);
  protected TSelf WithDescription(string description) => WithConfiguration(ControlInfoPropertyNames.Description, description);
}