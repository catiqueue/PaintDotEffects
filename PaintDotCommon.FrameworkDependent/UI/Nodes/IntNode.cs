using System.Collections.Generic;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using PropertyConfigEntry = System.Collections.Generic.KeyValuePair<PaintDotNet.IndirectUI.ControlInfoPropertyNames, object>;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public sealed class IntNode(string? name, int? defaultValue, Range<int> valueRange, IEnumerable<PropertyConfigEntry> configuration, PropertyControlType controlType = PropertyControlType.Slider)/* <TSettings> */ 
  : ValueNodeBase</* TSettings, */ int>(name ?? $"Int_{_nameCounter++}", defaultValue ?? valueRange.Start, configuration, controlType)
  // where TSettings : class 
{
  private static int _nameCounter = 0;

  private Range<int> ValueRange { get; } = valueRange;

  /* public IntDefinition(string? name, int? defaultValue, Range<int> valueRange, bool triggersRebuild, Expression<Func<TSettings, int>>? selector) 
    : base(name ?? $"IntParameter_{_nameCounter++}", defaultValue ?? valueRange.Start, triggersRebuild, selector) 
    => ValueRange = valueRange; */

  protected override Property ToProperty() => new Int32Property(Name, DefaultValue, ValueRange.Start, ValueRange.End);
}