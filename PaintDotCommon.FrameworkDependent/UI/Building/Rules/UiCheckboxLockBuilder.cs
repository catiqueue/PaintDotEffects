using System;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Rules;

public sealed class UiCheckboxLockBuilder<TParent> : UiLockBuilderBase<TParent, ReadOnlyBoundToBooleanRule>
{
  internal UiCheckboxLockBuilder(TParent parent) : base(parent) { }

  private Lazy<CheckboxNode>? _source;
  private CheckboxNode Source => _source?.Value ?? throw new IncompleteDefinitionException(nameof(UiCheckboxLockBuilder<TParent>), nameof(Source));
  private Lazy<UiNodeBase>? _target;
  private UiNodeBase Target => _target?.Value ?? throw new IncompleteDefinitionException(nameof(UiCheckboxLockBuilder<TParent>), nameof(Target));
  private bool? _inverse;
  private bool Inverse => _inverse ?? false;

  
  // TODO: I don't like them being public. That's to deal with.   // what?
  public UiCheckboxLockBuilder<TParent> WithSource(CheckboxNode source) => WithSource(() => source);
  internal UiCheckboxLockBuilder<TParent> WithSource(Func<CheckboxNode> source) {
    if (_source is not null) throw new ParameterDefinedException(nameof(UiCheckboxLockBuilder<TParent>), nameof(Source));
    _source = new Lazy<CheckboxNode>(source);
    return this;
  }
  
  public UiCheckboxLockBuilder<TParent> WithTarget(UiNodeBase target) => WithTarget(() => target);
  internal UiCheckboxLockBuilder<TParent> WithTarget(Func<UiNodeBase> target) {
    if (_target is not null) throw new ParameterDefinedException(nameof(UiCheckboxLockBuilder<TParent>), nameof(Target));
    _target = new Lazy<UiNodeBase>(target);
    return this;
  }

  public UiCheckboxLockBuilder<TParent> WhenChecked() {
    if(_inverse is not null) throw new ParameterDefinedException(nameof(UiCheckboxLockBuilder<TParent>), nameof(WhenChecked));
    _inverse = false;
    return this;
  }
  
  public UiCheckboxLockBuilder<TParent> WhenUnchecked() {
    if(_inverse is not null) throw new ParameterDefinedException(nameof(UiCheckboxLockBuilder<TParent>), nameof(WhenUnchecked));
    _inverse = true;
    return this;
  }

  protected override ReadOnlyBoundToBooleanRule Build() => new(Target.Name, Source.Name, Inverse);
}