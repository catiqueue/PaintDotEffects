using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building;

public sealed class UiIntParameterBuilder<TSettings, TParent> : UiValueBuilderBase<TSettings, TParent, UiIntParameterBuilder<TSettings, TParent>, IntNode/*<TSettings>*/, int> 
  where TSettings : class
{
  private Range<int>? _valueRange;
  private Range<int> ValueRange 
    => _valueRange 
       ?? throw new IncompleteDefinitionException(nameof(UiIntParameterBuilder<TSettings, TParent>), nameof(_valueRange));

  internal UiIntParameterBuilder(PluginUiBehaviorBuilder<TSettings> root, TParent parent) : base(root, parent) {
    ControlType = PropertyControlType.Slider;
  }
  
  public UiIntParameterBuilder<TSettings, TParent> WithValueRange(Range<int> valueRange) {
    _valueRange = valueRange;
    return DefaultSet ? this : WithDefault(valueRange.Start);
  }
  
  public override IntNode/*<TSettings>*/ Build() => new(
    Name,
    DefaultValue,
    ValueRange,
    Configuration,
    ControlType
    /* TriggersRebuild,
    Selector */);
}