using System.Collections.Generic;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public sealed class CheckboxNode(string? name, bool? defaultValue, IEnumerable<PropertyConfigEntry> configuration) 
  : ValueNodeBase<bool>(name ?? $"Checkbox_{_nameCounter++}", defaultValue ?? false, configuration, PropertyControlType.CheckBox) 
{
  private static int _nameCounter = 0;
  
  protected override Property ToProperty() => new BooleanProperty(Name, DefaultValue);
}