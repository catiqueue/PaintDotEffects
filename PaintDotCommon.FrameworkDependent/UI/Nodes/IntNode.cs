using System.Collections.Generic;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using PropertyConfigEntry = System.Collections.Generic.KeyValuePair<PaintDotNet.IndirectUI.ControlInfoPropertyNames, object>;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public sealed class IntNode(
  string? name, 
  int? defaultValue, 
  Range<int> valueRange, 
  IEnumerable<PropertyConfigEntry> configuration, 
  PropertyControlType controlType = PropertyControlType.Slider) 
  : ValueNodeBase<int>(name ?? $"Int_{_nameCounter++}", defaultValue ?? valueRange.Start, configuration, controlType)
{
  private static int _nameCounter = 0;

  private Range<int> ValueRange { get; } = valueRange;
  
  protected override Property ToProperty() => new Int32Property(Name, DefaultValue, ValueRange.Start, ValueRange.End);
}