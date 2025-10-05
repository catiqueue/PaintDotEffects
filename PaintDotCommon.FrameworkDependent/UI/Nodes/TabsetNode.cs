using System.Collections.Generic;
using System.Linq;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public sealed class TabsetNode(string? name, IEnumerable<TabNode> tabs) : UiNodeBase(name ?? $"Tabset_{_nameCounter++}") {
  private static int _nameCounter = 0;

  internal override IEnumerable<Property> GetProperties() => tabs.SelectMany(tab => tab.GetProperties()).Append(new TabContainerStateProperty(Name));

  internal override ControlInfo BuildControl(PropertyCollection properties) {
    var tabControls = tabs.Select(tab => (TabPageControlInfo) tab.BuildControl(properties));
    var tabset = new TabContainerControlInfo(properties.GetProperty<TabContainerStateProperty>(Name));
    foreach (var tab in tabControls) tabset.AddTab(tab);
    return tabset;
  }
}