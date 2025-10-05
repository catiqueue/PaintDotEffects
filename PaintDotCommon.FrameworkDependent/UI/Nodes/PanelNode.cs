using System.Collections.Generic;
using PaintDotNet.IndirectUI;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public sealed class PanelNode(string? name, IEnumerable<UiNodeBase> values) : ContainerNodeBase<PanelControlInfo>(name ?? "Panel", values) {
  protected override PanelControlInfo CreateContainer() => new();
  protected override void AppendFunction(PanelControlInfo container, ControlInfo child) 
    => container.AddChildControl(child);
}