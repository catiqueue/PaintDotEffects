using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public interface INodeBuilder<out TResult> where TResult : UiNodeBase {
  public TResult Result { get; }
}