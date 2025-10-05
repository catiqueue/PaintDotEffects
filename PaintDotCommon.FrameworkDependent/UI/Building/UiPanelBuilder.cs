using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public sealed class UiPanelBuilder<TSettings, TParent> : UiContainerBuilderBase<TSettings, TParent, PanelNode, PanelControlInfo, UiPanelBuilder<TSettings, TParent>> 
  where TSettings : class 
{
  internal UiPanelBuilder(PluginUiBehaviorBuilder<TSettings> root, TParent parent) : base(root, parent) { }
  protected override PanelNode Build() => new(Name, Items.Select(item => item.Result));
}