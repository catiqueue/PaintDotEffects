using System.Collections.Generic;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public abstract class UiNodeBase(string name) {
  public string Name { get; } = name;
  
  internal abstract IEnumerable<Property> GetProperties();
  internal abstract ControlInfo BuildControl(PropertyCollection properties);
}
