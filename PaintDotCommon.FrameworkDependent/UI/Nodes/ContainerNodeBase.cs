using System.Collections.Generic;
using System.Linq;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public abstract class ContainerNodeBase<TControl>(string name, IEnumerable<UiNodeBase> values) : UiNodeBase(name) 
  where TControl : ControlInfo
{
  internal sealed override IEnumerable<Property> GetProperties() => values.SelectMany(value => value.GetProperties());
 
  internal sealed override ControlInfo BuildControl(PropertyCollection properties) {
    var control = CreateContainer();
    foreach (var value in values) AppendFunction(control, value.BuildControl(properties));
    return control;
  }
  
  protected abstract TControl CreateContainer();
  protected abstract void AppendFunction(TControl container, ControlInfo child);
}