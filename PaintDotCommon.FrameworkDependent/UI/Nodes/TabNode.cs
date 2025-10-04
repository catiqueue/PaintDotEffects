using System.Collections.Generic;
using PaintDotNet.IndirectUI;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public sealed class TabNode/* <TSettings> */(string name, IEnumerable<UiNodeBase> values) : ContainerNodeBase <TabPageControlInfo/*, TSettings*/>(name, values) 
  // where TSettings : class 
{
  private readonly string _name = name;
  protected override TabPageControlInfo CreateContainer() => new() { Text = _name };
  protected override void AppendFunction(TabPageControlInfo tab, ControlInfo child) 
    => tab.AddChildControl(child);
} 