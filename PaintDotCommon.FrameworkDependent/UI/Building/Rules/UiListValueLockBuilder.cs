using System;
using System.Collections.Generic;
using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Rules;

public class UiListValueLockBuilder<TParent, TValue> : UiRuleBuilderBase<TParent, ReadOnlyBoundToValueRule<object, StaticListChoiceProperty>> where TValue : notnull {
  private Func<ChoiceListNode<TValue>>? _source;
  private ChoiceListNode<TValue> Source => _source?.Invoke() ?? throw new IncompleteDefinitionException(nameof(UiListValueLockBuilder<TParent, TValue>), nameof(Source));
  private Func<UiNodeBase>? _target;
  private UiNodeBase Target => _target?.Invoke() ?? throw new IncompleteDefinitionException(nameof(UiListValueLockBuilder<TParent, TValue>), nameof(Target));
  private bool? _inverse;
  private bool Inverse => _inverse ?? false;
  private HashSet<TValue>? _choices;
  private IEnumerable<TValue> Choices => _choices ?? throw new IncompleteDefinitionException(nameof(UiListValueLockBuilder<TParent, TValue>), nameof(Choices));
  
  internal UiListValueLockBuilder(TParent parent) : base(parent) { }
  
  public UiListValueLockBuilder<TParent, TValue> WithSource(ChoiceListNode<TValue> source) => WithSource(() => source);
  internal UiListValueLockBuilder<TParent, TValue> WithSource(Func<ChoiceListNode<TValue>> source) {
    if (_source is not null) throw new ParameterDefinedException(nameof(UiListValueLockBuilder<TParent, TValue>), nameof(Source));
    _source = source;
    return this;
  }
  
  public UiListValueLockBuilder<TParent, TValue> WithTarget(UiNodeBase target) => WithTarget(() => target);
  internal UiListValueLockBuilder<TParent, TValue> WithTarget(Func<UiNodeBase> target) {
    if (_target is not null) throw new ParameterDefinedException(nameof(UiListValueLockBuilder<TParent, TValue>), nameof(Target));
    _target = target;
    return this;
  }

  public UiListValueLockBuilder<TParent, TValue> WhenAnyOf(params TValue[] choices) {
    if(_choices is not null) throw new ParameterDefinedException(nameof(UiListValueLockBuilder<TParent, TValue>), nameof(Choices));
    _inverse = false;
    _choices = new HashSet<TValue>(choices);
    return this;
  }

  public UiListValueLockBuilder<TParent, TValue> WhenNotAnyOf(params TValue[] choices) {
    if(_choices is not null) throw new ParameterDefinedException(nameof(UiListValueLockBuilder<TParent, TValue>), nameof(Choices));
    _inverse = true;
    _choices = new HashSet<TValue>(choices);
    return this;
  }
  
  protected override ReadOnlyBoundToValueRule<object, StaticListChoiceProperty> Build() => new(Target.Name, Source.Name, Choices.Cast<object>().ToArray(), Inverse);
}