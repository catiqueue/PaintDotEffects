using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Binding;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Rules;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.Collections;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public sealed class PluginUiBehaviorBuilder<TSettings> where TSettings : class {
  private INodeBuilder<UiNodeBase>? _rootBuilder;
  private readonly List<Lazy<Binder<TSettings>>> _lazyBindings = [];
  private readonly List<Lazy<PropertyCollectionRule>> _lazyRules = [];
  private readonly List<Lazy<UiNodeBase>> _lazyTriggers = [];

  private UiNodeBase Root => _rootBuilder?.Result ?? throw new IncompleteDefinitionException(nameof(PluginUiBehaviorBuilder<TSettings>), "Root");
  private IEnumerable<Binder<TSettings>> Bindings => _lazyBindings.Select(x => x.Value);
  private IEnumerable<PropertyCollectionRule> Rules => _lazyRules.Select(x => x.Value);
  private IEnumerable<UiNodeBase> Triggers => _lazyTriggers.Select(x => x.Value);
  
  internal PluginUiBehaviorBuilder() {  }
  
  public UiPanelBuilder<TSettings, PluginUiBehaviorBuilder<TSettings>> FromPanel() 
    => _rootBuilder is null
      ? (UiPanelBuilder<TSettings, PluginUiBehaviorBuilder<TSettings>>)
        (_rootBuilder = new UiPanelBuilder<TSettings, PluginUiBehaviorBuilder<TSettings>>(this, this).WithName("Root"))
      : throw new MutuallyExclusiveException(nameof(FromPanel), nameof(FromTabset));

  public UiTabsetBuilder<TSettings, PluginUiBehaviorBuilder<TSettings>> FromTabset() {
    if (_rootBuilder is not null) throw new MutuallyExclusiveException(nameof(FromPanel), nameof(FromTabset));
    var builder = new UiTabsetBuilder<TSettings, PluginUiBehaviorBuilder<TSettings>>(this, this);
    _rootBuilder = builder;
    return builder;
  }
  
  public PluginUiBehaviorBuilder<TSettings> WithBinding<TValue>(ValueNodeBase<TValue> node, Expression<Func<TSettings, TValue>> selector) => WithBinding(() => node, selector);
  internal PluginUiBehaviorBuilder<TSettings> WithBinding<TValue>(Func<ValueNodeBase<TValue>> node, Expression<Func<TSettings, TValue>> selector) {
    var setter = Setter.Create(selector);
    _lazyBindings.Add(new Lazy<Binder<TSettings>>(() => Binder.CreateDirect(node(), setter)));
    return this;
  }
  
  public PluginUiBehaviorBuilder<TSettings> WithBinding<TValue, TTarget>(ValueNodeBase<TValue> node, Expression<Func<TSettings, TTarget>> selector, Func<TValue, TTarget> mutator) => WithBinding(() => node, selector, mutator);
  internal PluginUiBehaviorBuilder<TSettings> WithBinding<TValue, TTarget>(Func<ValueNodeBase<TValue>> node, Expression<Func<TSettings, TTarget>> selector, Func<TValue, TTarget> mutator) {
    var setter = Setter.Create(selector);
    _lazyBindings.Add(new Lazy<Binder<TSettings>>(() => Binder.CreateMutating(node(), setter, mutator)));
    return this;
  }
  
  public PluginUiBehaviorBuilder<TSettings> WithBinding<TValue1, TValue2, TTarget>
  (
    ValueNodeBase<TValue1> node1,
    ValueNodeBase<TValue2> node2,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TValue1, TValue2, TTarget> producer
  ) => WithBinding(() => node1, () => node2, selector, producer);
  internal PluginUiBehaviorBuilder<TSettings> WithBinding<TValue1, TValue2, TTarget>
  (
    Func<ValueNodeBase<TValue1>> node1,
    Func<ValueNodeBase<TValue2>> node2,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TValue1, TValue2, TTarget> producer
  ) {
    var setter = Setter.Create(selector);
    _lazyBindings.Add(new Lazy<Binder<TSettings>>(() => Binder.CreateComplex(node1(), node2(), setter, producer)));
    return this;
  }
  
  public PluginUiBehaviorBuilder<TSettings> WithBinding<TValue1, TValue2, TValue3, TTarget>
  (
    ValueNodeBase<TValue1> node1,
    ValueNodeBase<TValue2> node2,
    ValueNodeBase<TValue3> node3,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TValue1, TValue2, TValue3, TTarget> producer
  ) => WithBinding(() => node1, () => node2, () => node3, selector, producer);
  internal PluginUiBehaviorBuilder<TSettings> WithBinding<TValue1, TValue2, TValue3, TTarget>
  (
    Func<ValueNodeBase<TValue1>> node1,
    Func<ValueNodeBase<TValue2>> node2,
    Func<ValueNodeBase<TValue3>> node3,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TValue1, TValue2, TValue3, TTarget> producer
  ) {
    var setter = Setter.Create(selector);
    _lazyBindings.Add(new Lazy<Binder<TSettings>>(() => Binder.CreateComplex(node1(), node2(), node3(), setter, producer)));
    return this;
  }
  
  public PluginUiBehaviorBuilder<TSettings> WithBinding<TValue1, TValue2, TValue3, TValue4, TTarget>
  (
    ValueNodeBase<TValue1> node1,
    ValueNodeBase<TValue2> node2,
    ValueNodeBase<TValue3> node3,
    ValueNodeBase<TValue4> node4,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TValue1, TValue2, TValue3, TValue4, TTarget> producer
  ) => WithBinding(() => node1, () => node2, () => node3, () => node4, selector, producer);
  internal PluginUiBehaviorBuilder<TSettings> WithBinding<TValue1, TValue2, TValue3, TValue4, TTarget>
  (
    Func<ValueNodeBase<TValue1>> node1,
    Func<ValueNodeBase<TValue2>> node2,
    Func<ValueNodeBase<TValue3>> node3,
    Func<ValueNodeBase<TValue4>> node4,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TValue1, TValue2, TValue3, TValue4, TTarget> producer
  ) {
    var setter = Setter.Create(selector);
    _lazyBindings.Add(new Lazy<Binder<TSettings>>(() => Binder.CreateComplex(node1(), node2(), node3(), node4(), setter, producer)));
    return this;
  }
  
  public PluginUiBehaviorBuilder<TSettings> WithBinding(TabsetNode node, Expression<Func<TSettings, int>> selector) => WithBinding(() => node, selector);
  internal PluginUiBehaviorBuilder<TSettings> WithBinding(Func<TabsetNode> node, Expression<Func<TSettings, int>> selector) {
    var setter = Setter.Create(selector);
    _lazyBindings.Add(new Lazy<Binder<TSettings>>(() => Binder.CreateForTabNumber(node(), setter)));
    return this;
  }

  public PluginUiBehaviorBuilder<TSettings> WithTriggeringProperty<TValue>(ValueNodeBase<TValue> node) => WithTriggeringProperty(() => node);
  internal PluginUiBehaviorBuilder<TSettings> WithTriggeringProperty<TValue>(Func<ValueNodeBase<TValue>> node) {
    _lazyTriggers.Add(new(node));
    return this;
  }

  public UiCheckboxLockBuilder<PluginUiBehaviorBuilder<TSettings>> WithCheckboxReadonlyRule() => WithCheckboxReadonlyRule(this);
  internal UiCheckboxLockBuilder<TParent> WithCheckboxReadonlyRule<TParent>(TParent parent) {
    var ruleBuilder = new UiCheckboxLockBuilder<TParent>(parent);
    _lazyRules.Add(new(() => ruleBuilder.Result));
    return ruleBuilder;
  }

  public UiListValueLockBuilder<PluginUiBehaviorBuilder<TSettings>, TValue> WithListValueReadonlyRule<TValue>() where TValue : notnull 
    => WithListValueReadonlyRule<PluginUiBehaviorBuilder<TSettings>, TValue>(this);
  internal UiListValueLockBuilder<TParent, TValue> WithListValueReadonlyRule<TParent, TValue>(TParent parent) where TValue : notnull {
    var ruleBuilder = new UiListValueLockBuilder<TParent, TValue>(parent);
    _lazyRules.Add(new(() => ruleBuilder.Result));
    return ruleBuilder;
  }

  public PluginUiBehaviorModel<TSettings> Build() => new(Root, Bindings, Triggers, Rules);
}