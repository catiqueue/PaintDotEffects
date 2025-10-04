using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public interface INodeBuilder</*TSettings, */out TResult> 
  where TResult : UiNodeBase // <TSettings> 
{
  public TResult Result { get; }
  public TResult Build(); 
}