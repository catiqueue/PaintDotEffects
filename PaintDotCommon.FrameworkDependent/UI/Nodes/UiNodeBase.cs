using System;
using System.Collections.Generic;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
// using catiqueue.PaintDotNet.Plugins.Common.UI.Binding;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public abstract class UiNodeBase/* <TSettings> */(string name) {
  public string Name { get; } = name; // string.Join(string.Empty, name.Split(Null.Ref<string[]>(), StringSplitOptions.TrimEntries));
  
  // public abstract IEnumerable<PropertyCollectionRule> GetRules();
  internal abstract IEnumerable<Property> GetProperties();
  // public abstract IEnumerable<Binder<TSettings>> GetBindings();
  // public abstract IEnumerable<string> GetTriggeringPropertyNames();
  internal abstract ControlInfo BuildControl(PropertyCollection properties);
}

// I was just curious
public static class Null {
  public static T? Ref<T>() where T : class => null;
  public static T? Val<T>() where T : struct => null;
}