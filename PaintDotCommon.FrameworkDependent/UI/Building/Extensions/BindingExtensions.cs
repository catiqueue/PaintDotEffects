using System;
using System.Linq.Expressions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Extensions;

public static class BindingExtensions {
  public static TSelf BindTo<TSettings, TParent, TSelf, TResult, TValue> 
  (
    this UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> builder, 
    Expression<Func<TSettings, TValue>> selector
  ) 
    where TSettings : class
    where TSelf : UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> 
    where TResult : ValueNodeBase<TValue> 
  {
    builder.Root.WithBinding(() => builder.Result, selector);
    return (TSelf) builder;
  }
  
  
  
  public static TSelf BindTo<TSettings, TParent, TSelf, TResult, TValue, TTarget> 
  (
    this UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> builder, 
    Expression<Func<TSettings, TTarget>> selector,
    Func<TValue, TTarget> mutator
  ) 
    where TSettings : class
    where TSelf : UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> 
    where TResult : ValueNodeBase<TValue> 
  {
    builder.Root.WithBinding(() => builder.Result, selector, mutator);
    return (TSelf) builder;
  }
  
  

  public static UiTabsetBuilder<TSettings, TParent> BindPageNumberTo<TSettings, TParent>(
    this UiTabsetBuilder<TSettings, TParent> tabset, Expression<Func<TSettings, int>> selector) 
    where TSettings : class 
  {
    tabset.Root.WithBinding(() => tabset.Result, selector);
    return tabset;
  }
}