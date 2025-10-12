using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Rules;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Values;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Extensions;

public static class RuleExtensions {
  public static UiCheckboxLockBuilder<TSelf> LockedBy<TSettings, TParent, TSelf, TResult, TValue>
  (
    this UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> builder,
    CheckboxNode checkbox
  ) 
    where TSettings : class 
    where TSelf : UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> 
    where TResult : ValueNodeBase<TValue> 
  => builder.Root.WithCheckboxReadonlyRule((TSelf) builder).WithSource(checkbox).WithTarget(() => builder.Result);
  
  public static UiCheckboxLockBuilder<UiCheckboxBuilder<TSettings, TParent>> Locks<TSettings, TParent, TValue>
  (
    this UiCheckboxBuilder<TSettings, TParent> builder,
    ValueNodeBase<TValue> value
  )
    where TSettings : class
  => builder.Root.WithCheckboxReadonlyRule(builder).WithSource(() => builder.Result).WithTarget(value);

  public static UiListValueLockBuilder<TSelf, TChoice> LockedBy<TSettings, TParent, TSelf, TResult, TValue, TChoice>
  (
    this UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> builder,
    ChoiceListNode<TChoice> value
  )
    where TChoice : notnull
    where TSettings : class
    where TSelf : UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue>
    where TResult : ValueNodeBase<TValue> 
  => builder.Root.WithListValueReadonlyRule<TSelf, TChoice>((TSelf) builder).WithSource(value).WithTarget(() => builder.Result);
  
  public static UiListValueLockBuilder<UiChoiceListBuilder<TSettings, TParent, TChoice>, TChoice> Locks<TSettings, TParent, TValue, TChoice>
  (
    this UiChoiceListBuilder<TSettings, TParent, TChoice> builder,
    ValueNodeBase<TValue> value
  )
    where TSettings : class
    where TValue : notnull
    where TChoice : notnull
  => builder.Root.WithListValueReadonlyRule<UiChoiceListBuilder<TSettings, TParent, TChoice>, TChoice>(builder).WithSource(() => builder.Result).WithTarget(value);
}