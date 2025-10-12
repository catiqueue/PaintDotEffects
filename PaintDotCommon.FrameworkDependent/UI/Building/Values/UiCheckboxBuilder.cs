using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Values;

public sealed class UiCheckboxBuilder<TSettings, TParent> 
  : UiValueBuilderBase<TSettings, TParent, UiCheckboxBuilder<TSettings, TParent>, CheckboxNode, bool> 
  where TSettings : class 
{
  internal UiCheckboxBuilder(PluginUiBehaviorBuilder<TSettings> root, TParent parent) : base(root, parent) { }
  protected override void OnSetName(string name) => WithDisplayName("").WithDescription(name);
  protected override CheckboxNode Build() => new(Name, DefaultValue, Configuration);
}