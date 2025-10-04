using System.Collections.Generic;
using System.Linq;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public abstract class ContainerNodeBase<TControl /*, TSettings*/>(string name, IEnumerable<UiNodeBase> values) : UiNodeBase/* <TSettings> */(name) 
  // where TSettings : class
  where TControl : ControlInfo
{
  // private readonly List<UiDefinitionBase<TSettings>> _values = [];

  /* public void Add(UiDefinitionBase<TSettings> value) {
    switch(value) {
      case IntDefinition<TSettings> or TabsetDefinition<TSettings>: _values.Add(value); break;
      case PanelDefinition<TSettings>: throw new InvalidCastException("Cannot add a panel to a container.");
      case TabDefinition<TSettings>: throw new InvalidCastException("Cannot add a tab to a container.");
      default: throw new InvalidCastException("Cannot add value of type " + value.GetType().Name);
    }
  } */

  internal sealed override IEnumerable<Property> GetProperties() => values.SelectMany(value => value.GetProperties());
  // public sealed override IEnumerable<PropertyCollectionRule> GetRules() => _values.SelectMany(val => val.GetRules());
  // public sealed override IEnumerable<Binder<TSettings>> GetBindings() => _values.SelectMany(val => val.GetBindings());
  // public sealed override IEnumerable<string> GetTriggeringPropertyNames() => _values.SelectMany(val => val.GetTriggeringPropertyNames());

  internal sealed override ControlInfo BuildControl(PropertyCollection properties) {
    var control = CreateContainer();
    foreach (var value in values) AppendFunction(control, value.BuildControl(properties));
    return control;
  }
  
  protected abstract TControl CreateContainer();
  protected abstract void AppendFunction(TControl container, ControlInfo child);
}