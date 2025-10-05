using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public sealed class UiTabBuilder<TSettings, TParent> : UiContainerBuilderBase<TSettings, TParent, TabNode, TabPageControlInfo, UiTabBuilder<TSettings, TParent>> 
  where TSettings : class 
{
  internal UiTabBuilder(PluginUiBehaviorBuilder<TSettings> root, TParent parent) : base(root, parent) { }

  protected override TabNode Build() => new(
    Name ?? throw new IncompleteDefinitionException(
      nameof(UiTabBuilder<TSettings, TParent>), 
      nameof(Name)), 
    Items.Select(item => item.Result));
}
