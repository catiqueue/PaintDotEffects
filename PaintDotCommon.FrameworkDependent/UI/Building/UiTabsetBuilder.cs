using System.Collections.Generic;
using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public sealed class UiTabsetBuilder<TSettings, TParent> : UiBuilderBase<TSettings, TParent, UiTabsetBuilder<TSettings, TParent>, TabsetNode> 
  where TSettings : class 
{
  internal UiTabsetBuilder(PluginUiBehaviorBuilder<TSettings> root, TParent parent) : base(root, parent) { }
  
  private readonly List<UiTabBuilder<TSettings, UiTabsetBuilder<TSettings, TParent>>> _tabs = [];

  public UiTabBuilder<TSettings, UiTabsetBuilder<TSettings, TParent>> WithTab(string name) {
    var tab = new UiTabBuilder<TSettings, UiTabsetBuilder<TSettings, TParent>>(Root, this).WithName(name);
    _tabs.Add(tab);
    return tab;
  }
  
  public override TabsetNode Build() => new(Name, _tabs.Select(tab => tab.Result));
}