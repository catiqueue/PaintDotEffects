using System.Collections.Generic;
using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using PaintDotNet.Collections;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using PropertyConfigEntry = System.Collections.Generic.KeyValuePair<PaintDotNet.IndirectUI.ControlInfoPropertyNames, object>;

namespace catiqueue.PaintDotNet.Plugins.Common.UI;

public static class PropertyExtensions {
  public static PropertyControlInfo SetType(this PropertyControlInfo control, PropertyControlType type) {
    if(type != PropertyControlType.Null)
      control.ControlType.Value = type;
    return control;
  }
  
  public static PropertyControlInfo ApplyConfiguration(this PropertyControlInfo control, IEnumerable<PropertyConfigEntry> configuration) {
    configuration.Select(kvp => KeyValuePair.Create(control.ControlProperties[kvp.Key], kvp.Value))
      .Where(kvp => kvp.Key is not null)
      .ForEach(kvp => kvp.Key!.Value = kvp.Value);
    return control;
  }
  
  public static PropertyControlInfo CreateControl(this Property property) 
    => PropertyControlInfo.CreateFor(property);
  
  public static Property GetProperty(this PropertyCollection properties, string name) 
    => properties[name] ?? throw new PropertyAccessException(name);
  
  public static T GetProperty<T>(this PropertyCollection properties, string name) where T : Property 
    => properties.GetProperty(name) as T ?? throw new PropertyAccessException($"{name} ({typeof(T).Name})");
  
  public static T GetPropertyValue<T>(this PropertyCollection properties, string name) 
    => (T) (properties.GetProperty(name).Value ?? throw new PropertyAccessException(name));
}