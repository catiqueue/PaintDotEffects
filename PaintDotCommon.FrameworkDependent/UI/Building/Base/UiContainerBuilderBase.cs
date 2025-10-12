using System.Collections.Generic;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Values;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;

public abstract class UiContainerBuilderBase<TSettings, TParent, TResult, TControl, TSelf>(PluginUiBehaviorBuilder<TSettings> root, TParent parent)
  : UiBuilderBase<TSettings, TParent, TSelf, TResult>(root, parent)
  where TControl : ControlInfo
  where TResult : ContainerNodeBase<TControl>
  where TSelf : UiContainerBuilderBase<TSettings, TParent, TResult, TControl, TSelf>
  where TSettings : class
{
  private readonly List<INodeBuilder<UiNodeBase>> _items = [];
  protected IEnumerable<INodeBuilder<UiNodeBase>> Items => _items;

  public UiIntParameterBuilder<TSettings, TSelf> WithIntegerSlider(string name) => WithIntegerSlider().WithName(name);
  public UiIntParameterBuilder<TSettings, TSelf> WithIntegerSlider() {
    var builder = new UiIntParameterBuilder<TSettings, TSelf>(Root, (TSelf) this);
    _items.Add(builder);
    return builder;
  }

  public UiCheckboxBuilder<TSettings, TSelf> WithCheckbox(string name) => WithCheckbox().WithName(name);
  public UiCheckboxBuilder<TSettings, TSelf> WithCheckbox() {
    var builder = new UiCheckboxBuilder<TSettings, TSelf>(Root, (TSelf) this);
    _items.Add(builder);
    return builder;
  }
  
  public UiChoiceListBuilder<TSettings, TSelf, TValue> WithChoiceList<TValue>(string name) where TValue : notnull 
    => WithChoiceList<TValue>().WithName(name);
  public UiChoiceListBuilder<TSettings, TSelf, TValue> WithChoiceList<TValue>() where TValue : notnull {
    var builder = new UiChoiceListBuilder<TSettings, TSelf, TValue>(Root, (TSelf)this);
    _items.Add(builder);
    return builder;
  }

  public UiColorWheelBuilder<TSettings, TSelf> WithColorPicker(string name) => WithColorPicker().WithName(name);
  public UiColorWheelBuilder<TSettings, TSelf> WithColorPicker() {
    var builder = new UiColorWheelBuilder<TSettings, TSelf>(Root, (TSelf) this);
    _items.Add(builder);
    return builder;
  }

  public UiTabsetBuilder<TSettings, TSelf> WithTabset(string name) => WithTabset().WithName(name);
  public UiTabsetBuilder<TSettings, TSelf> WithTabset() {
    var builder = new UiTabsetBuilder<TSettings, TSelf>(Root, (TSelf) this);
    _items.Add(builder);
    return builder;
  }
}