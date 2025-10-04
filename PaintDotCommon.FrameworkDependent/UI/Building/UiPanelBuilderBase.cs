using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public sealed class UiPanelBuilderBase<TSettings, TParent> : UiContainerBuilderBase<TSettings, TParent, PanelNode/*<TSettings>*/, PanelControlInfo, UiPanelBuilderBase<TSettings, TParent>> 
  where TSettings : class 
{
  internal UiPanelBuilderBase(PluginUiBehaviorBuilder<TSettings> root, TParent parent) : base(root, parent) { }
  public override PanelNode/*<TSettings>*/ Build() => new /*<TSettings>*/(Name, Items.Select(item => item.Result));
}