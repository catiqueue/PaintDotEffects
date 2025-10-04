using System.Collections.Generic;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using PropertyConfigEntry = System.Collections.Generic.KeyValuePair<PaintDotNet.IndirectUI.ControlInfoPropertyNames, object>;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public abstract class ValueNodeBase</*TSettings,*/ TValue>(string name, TValue defaultValue, IEnumerable<PropertyConfigEntry> configuration, PropertyControlType controlType = PropertyControlType.Null /*, bool triggersRebuild, Expression<Func<TSettings, TValue>>? selector */)
  : UiNodeBase/* <TSettings> */(name) 
  // where TSettings : class 
{
  //private Expression<Func<TSettings, TValue>>? Selector { get; } = selector;
  
  protected TValue DefaultValue { get; } = defaultValue;
  // protected bool TriggersRebuild { get; } = triggersRebuild;

  protected abstract Property ToProperty();
  internal override IEnumerable<Property> GetProperties() => new[] { ToProperty() };

  // public sealed override IEnumerable<Binder<TSettings>> GetBindings() => Selector is not null ? [Binder.CreateDirect(Name, Selector)] : [];
  // public sealed override IEnumerable<string> GetTriggeringPropertyNames() => TriggersRebuild ? [Name] : [];

  // private readonly List<PropertyCollectionRule> _rules = [];
  // public void AddRule(PropertyCollectionRule rule) => _rules.Add(rule);
  // public sealed override IEnumerable<PropertyCollectionRule> GetRules() => _rules;

  // private readonly Dictionary<ControlInfoPropertyNames, object> _configuration = [];
  // public void Configure(ControlInfoPropertyNames name, object value) => _configuration[name] = value;

  // private PropertyControlType _type = PropertyControlType.Null;
  // public void SetType(PropertyControlType type) => _type = type;

  internal sealed override ControlInfo BuildControl(PropertyCollection properties) 
    => properties.GetProperty(Name).CreateControl().ApplyConfiguration(configuration).SetType(controlType);
}