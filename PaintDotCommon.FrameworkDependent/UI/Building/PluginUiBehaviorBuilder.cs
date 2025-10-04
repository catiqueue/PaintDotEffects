using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Binding;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.Collections;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public sealed class PluginUiBehaviorBuilder<TSettings>
  where TSettings : class 
{
  private INodeBuilder<UiNodeBase>? _rootBuilder;
  private readonly List<Lazy<Binder<TSettings>>> _lazyBindings = [];
  private readonly List<Lazy<PropertyCollectionRule>> _lazyRules = [];
  private readonly List<Lazy<UiNodeBase>> _lazyTriggers = [];

  private UiNodeBase Root => _rootBuilder?.Result ?? throw new IncompleteDefinitionException(nameof(PluginUiBehaviorBuilder<TSettings>), "Root");
  private IEnumerable<Binder<TSettings>> Bindings => _lazyBindings.Select(x => x.Value);
  private IEnumerable<PropertyCollectionRule> Rules => _lazyRules.Select(x => x.Value);
  private IEnumerable<UiNodeBase> Triggers => _lazyTriggers.Select(x => x.Value);
  
  internal PluginUiBehaviorBuilder() {  }
  
  public UiPanelBuilderBase<TSettings, PluginUiBehaviorBuilder<TSettings>> FromPanel() {
    return _rootBuilder is null
      ? (UiPanelBuilderBase<TSettings, PluginUiBehaviorBuilder<TSettings>>)
        (_rootBuilder = new UiPanelBuilderBase<TSettings, PluginUiBehaviorBuilder<TSettings>>(this, this).WithName("Root"))
      : throw new MutuallyExclusiveException(nameof(FromPanel), nameof(FromTabset));
  }
  
  // TODO: Implement tabset builder
  public UiPanelBuilderBase<TSettings, PluginUiBehaviorBuilder<TSettings>> FromTabset() {
    if (_rootBuilder is not null) throw new MutuallyExclusiveException(nameof(FromPanel), nameof(FromTabset));
    var builder = new UiPanelBuilderBase<TSettings, PluginUiBehaviorBuilder<TSettings>>(this, this);
    _rootBuilder = builder;
    return builder;
  }

  public PluginUiBehaviorBuilder<TSettings> WithDirectBinding<TValue>(ValueNodeBase<TValue> node, Expression<Func<TSettings, TValue>> selector) {
    var binder = Binder.CreateDirect(node, selector);
    _lazyBindings.Add(new Lazy<Binder<TSettings>>(binder));
    return this;
  }
  
  internal void AddDirectBinding<TValue>(INodeBuilder<ValueNodeBase<TValue>> nodeBuilder, Expression<Func<TSettings, TValue>> selector) 
    => _lazyBindings.Add(new Lazy<Binder<TSettings>>(() => Binder.CreateDirect(nodeBuilder.Result, selector)));

  public PluginUiBehaviorBuilder<TSettings> WithTriggeringProperty<TValue>(ValueNodeBase<TValue> node) {
    _lazyTriggers.Add(new(() => node));
    return this;
  }
  
  internal void AddTriggeringProperty<TValue>(INodeBuilder<ValueNodeBase<TValue>> nodeBuilder) 
    => _lazyTriggers.Add(new(() => nodeBuilder.Result));

  // TODO: Implement rules adequately
  public PluginUiBehaviorBuilder<TSettings> WithRule(PropertyCollectionRule rule) {
    _lazyRules.Add(new(rule));
    return this;
  }
  
  public PluginUiBehaviorModel<TSettings> Build() => new(Root, Bindings, Triggers, Rules);
}