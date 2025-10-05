using System;
using System.Collections.Generic;
using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.UI.Binding;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI;

public class PluginUiBehaviorModel<TSettings> where TSettings : class 
{
  private UiNodeBase Root { get; }
  internal Binder<TSettings>[] Bindings { get; }
  internal UiNodeBase[] TriggeringProperties { get; }
  internal PropertyCollectionRule[] Rules { get; }
  internal Property[] Properties { get; }
  
  public PluginUiBehaviorModel(
    UiNodeBase root, 
    IEnumerable<Binder<TSettings>> bindings, 
    IEnumerable<UiNodeBase> triggeringProperties, 
    IEnumerable<PropertyCollectionRule> rules) 
  {
    // maybe support adding everything as root and then create a panel for it (or a tabset, in case of a tab)
    // but it's good enough as it is right now
    Root = root switch {
      PanelNode or TabsetNode => root,
      _ => throw new InvalidCastException("Root must be a panel or a tabset.")
    };
    Properties = Root.GetProperties().ToArray();
    Bindings = bindings.ToArray();
    TriggeringProperties = triggeringProperties.ToArray();
    Rules = rules.ToArray();
  }

  // Apparently, caching controls is a bad idea.
  internal ControlInfo GetControl(PropertyCollection properties)
    => Root.BuildControl(properties);
}