using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Extensions;

public static class MiscExtensions {
  public static TSelf ChangeTriggersRebuild<TSettings, TParent, TSelf, TResult, TValue>
  (
    this UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> builder
  )
    where TSettings : class
    where TSelf : UiValueBuilderBase<TSettings, TParent, TSelf, TResult, TValue> 
    where TResult : ValueNodeBase<TValue> 
  {
    builder.Root.WithTriggeringProperty(() => builder.Result);
    return (TSelf) builder;
  }
}