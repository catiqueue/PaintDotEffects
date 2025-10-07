using System;
using System.Collections.Generic;
using System.Linq;
using PaintDotNet.Collections;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public sealed class ChoiceListNode<TValue> : ValueNodeBase<TValue>
  where TValue : notnull 
{
  public readonly record struct Choice(TValue Value, string Name);
  
  private static int _nameCounter = 0;
  private readonly Choice[] _choices;
  private readonly int _defaultIndex;
  
  public ChoiceListNode(string? name, TValue defaultValue, IEnumerable<TValue> choices, IEnumerable<PropertyConfigEntry> configuration)
    : this(name, defaultValue, choices.Select(value => new Choice(value, value.ToString() ?? string.Empty)), configuration) { }

  public ChoiceListNode(string? name, TValue defaultValue, IEnumerable<Choice> choices, IEnumerable<PropertyConfigEntry> configuration) 
    : base(name ?? $"ChoiceList_{_nameCounter++}", defaultValue, configuration, PropertyControlType.DropDown) 
  {
    _choices = choices.ToArray();
    _defaultIndex = _choices.Select(choice => choice.Value).IndexOf(defaultValue);
    if(_defaultIndex == -1) throw new ArgumentOutOfRangeException(nameof(defaultValue));
  }

  protected override Property ToProperty() 
    => new StaticListChoiceProperty(Name, _choices.Select(choice => choice.Value).Cast<object>().ToArray(), _defaultIndex);

  protected override ControlInfo OnBuildControl(PropertyControlInfo control) {
    _choices.ForEach(choice => control.SetValueDisplayName(choice.Value, choice.Name));
    return control;
  }
}