using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Values;

public class UiColorWheelBuilder<TSettings, TParent> 
  : UiValueBuilderBase<TSettings, TParent, UiColorWheelBuilder<TSettings, TParent>, ColorNode, ManagedColor> 
  where TSettings : class 
{
  internal UiColorWheelBuilder(PluginUiBehaviorBuilder<TSettings> root, TParent parent) : base(root, parent) { }

  protected override ColorNode Build() => new(Name, DefaultValue, Configuration);
}