using System.Collections.Generic;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public abstract class UiContainerBuilderBase<TSettings, TParent, TResult, TControl, TSelf>(PluginUiBehaviorBuilder<TSettings> root, TParent parent)
  : UiBuilderBase<TSettings, TParent, TSelf, TResult>(root, parent)
  where TControl : ControlInfo
  where TResult : ContainerNodeBase<TControl>/*<TSettings>*/
  where TSelf : UiContainerBuilderBase<TSettings, TParent, TResult, TControl, TSelf>
  where TSettings : class
{
  private readonly List<INodeBuilder</*TSettings,*/ UiNodeBase/*<TSettings>*/>> _items = [];
  protected IEnumerable<INodeBuilder</*TSettings,*/ UiNodeBase/*<TSettings>*/>> Items => _items;

  public UiIntParameterBuilder<TSettings, TSelf> WithInt() {
    var builder = new UiIntParameterBuilder<TSettings, TSelf>(Root, (TSelf) this);
    _items.Add(builder);
    return builder;
  }
  
  public UiEnumListBuilder<TSettings, TSelf, TValue> WithChoiceList<TValue>() where TValue : notnull {
    var builder = new UiEnumListBuilder<TSettings, TSelf, TValue>(Root, (TSelf)this);
    _items.Add(builder);
    return builder;
  }
  
  public UiTabsetBuilder<TSettings, TSelf> WithTabset() {
    var builder = new UiTabsetBuilder<TSettings, TSelf>(Root, (TSelf) this);
    _items.Add(builder);
    return builder;
  }
}