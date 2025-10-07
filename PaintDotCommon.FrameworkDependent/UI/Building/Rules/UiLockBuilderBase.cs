using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Rules;

public abstract class UiLockBuilderBase<TParent, TResult>(TParent parent) where TResult : PropertyCollectionRule {
  private TResult? _result;
  public TResult Result => _result ??= Build();
  
  protected abstract TResult Build();

  public TParent Then() {
    _result ??= Build();  
    return parent;
  }
}