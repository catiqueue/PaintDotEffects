using System.Diagnostics.CodeAnalysis;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;

public abstract class UiBuilderBase<TSettings, TParent, TSelf, TResult>(PluginUiBehaviorBuilder<TSettings> root, TParent parent)
  : INodeBuilder<TResult>
  where TResult : UiNodeBase 
  where TSettings : class
  where TSelf : UiBuilderBase<TSettings, TParent, TSelf, TResult>
{
  private TResult? _result;
  public TResult Result => _result ??= Build();
  
  internal PluginUiBehaviorBuilder<TSettings> Root => root;
  internal TParent Parent => parent;
  
  protected string? Name { get; private set; }
  
  internal TSelf WithName(string name) {
    Name = name;
    return (TSelf) this;
  }

  protected abstract TResult Build();

  public TParent Then() {
    _result ??= Build();  
    return parent;
  }

  public TParent Then(out TResult result) {
    result = _result ??= Build();
    return parent;
  }

  public PluginUiBehaviorBuilder<TSettings> End() {
    _result ??= Build();
    return root;
  }
  
  public PluginUiBehaviorBuilder<TSettings> End(out TResult result) {
    result = _result ??= Build();
    return root;
  }
}