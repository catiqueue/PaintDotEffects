using System.Collections.Generic;
using System.Linq;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public sealed class TabsetNode/*<TSettings>*/(string? name, IEnumerable<TabNode/*<TSettings>*/> tabs /*bool triggersRebuild, Expression<Func<TSettings, TabContainerState>>? selector*/)
  : UiNodeBase/* <TSettings>*/(name ?? $"Tabset_{_nameCounter++}") 
  // where TSettings : class 
{
  private static int _nameCounter = 0;
  
  // private readonly List<TabDefinition<TSettings>> _tabs = [];
  // public void Add(TabDefinition<TSettings> tab) => _tabs.Add(tab);

  internal override IEnumerable<Property> GetProperties() => tabs.SelectMany(tab => tab.GetProperties()).Append(new TabContainerStateProperty(Name));
  // public override IEnumerable<PropertyCollectionRule> GetRules() => _tabs.SelectMany(tab => tab.GetRules());
  // public override IEnumerable<Binder<TSettings>> GetBindings() {
    // var children = _tabs.SelectMany(tab => tab.GetBindings());
    // return selector is not null ? children.Append(Binder.CreateDirect(Name, selector)) : children;
  // }
  // public override IEnumerable<string> GetTriggeringPropertyNames() {
    // var children = _tabs.SelectMany(tab => tab.GetTriggeringPropertyNames());
    // return triggersRebuild ? children.Append(Name) : children;
  // }

  internal override ControlInfo BuildControl(PropertyCollection properties) {
    var tabControls = tabs.Select(tab => (TabPageControlInfo) tab.BuildControl(properties));
    var tabset = new TabContainerControlInfo(properties.GetProperty<TabContainerStateProperty>(Name));
    foreach (var tab in tabControls) tabset.AddTab(tab);
    return tabset;
  }
}