using System;
using System.Linq.Expressions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Binding;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public static class Extensions {
  public static TSelf BindTo<TSettings, TParent, TSelf, TResult, TValue> 
  (
    this UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> builder, 
    Expression<Func<TSettings, TValue>> selector
  ) 
    where TSettings : class
    where TSelf : UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> 
    where TResult : ValueNodeBase<TValue> 
  {
    builder.Root.AddDirectBinding(builder, Setter.Create(selector));
    return (TSelf) builder;
  }
  
  public static TSelf ChangeTriggersRebuild<TSettings, TParent, TSelf, TResult, TValue>
  (
    this UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> builder
  )
    where TSettings : class
    where TSelf : UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> 
    where TResult : ValueNodeBase<TValue> 
  {
    builder.Root.AddTriggeringProperty(builder);
    return (TSelf) builder;
  }
}