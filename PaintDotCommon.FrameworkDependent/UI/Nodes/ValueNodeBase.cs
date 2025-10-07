using System.Collections.Generic;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public readonly record struct PropertyConfigEntry(ControlInfoPropertyNames Key, object Value);

public abstract class ValueNodeBase<TValue>(
  string name, 
  TValue defaultValue, 
  IEnumerable<PropertyConfigEntry> configuration, 
  PropertyControlType controlType = PropertyControlType.Null)
  : UiNodeBase(name) 
{
  protected TValue DefaultValue { get; } = defaultValue;

  protected abstract Property ToProperty();
  internal sealed override IEnumerable<Property> GetProperties() => new[] { ToProperty() };
  
  protected virtual ControlInfo OnBuildControl(PropertyControlInfo control) => control;

  internal sealed override ControlInfo BuildControl(PropertyCollection properties) 
    => OnBuildControl(properties.GetProperty(Name).CreateControl().ApplyConfiguration(configuration).SetType(controlType));
}