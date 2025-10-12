using System.Collections.Generic;
using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Values;

public sealed class UiChoiceListBuilder<TSettings, TParent, TValue> 
  : UiValueBuilderBase<TSettings, TParent, UiChoiceListBuilder<TSettings, TParent, TValue>, ChoiceListNode<TValue>, TValue> 
  where TSettings : class 
  where TValue : notnull 
{
  // TODO: use ChoiceListNode<TValue>.Choice hashset.
  private readonly Dictionary<TValue, string> _choices = []; 
  
  internal UiChoiceListBuilder(PluginUiBehaviorBuilder<TSettings> root, TParent parent) : base(root, parent) { }
  
  public UiChoiceListBuilder<TSettings, TParent, TValue> WithChoice(TValue value, string name, out TValue choice) {
    choice = value;
    _choices[value] = name;
    return this;
  }
  
  public UiChoiceListBuilder<TSettings, TParent, TValue> WithChoice(TValue value, out TValue choice) 
    => WithChoice(value, value.ToString() ?? string.Empty, out choice);
  
  public UiChoiceListBuilder<TSettings, TParent, TValue> WithChoice(TValue value, string name) {
    _choices[value] = name;
    return this;
  }
  
  public UiChoiceListBuilder<TSettings, TParent, TValue> WithChoice(TValue value) 
    => WithChoice(value, value.ToString() ?? string.Empty);

  protected override ChoiceListNode<TValue> Build() {
    return new ChoiceListNode<TValue>(
      Name,
      _choices.FirstOrDefault() is var choice ? choice.Key : throw new IncompleteDefinitionException(nameof(UiChoiceListBuilder<TSettings, TParent, TValue>), "Choices"),
      _choices.Select(kvp => new ChoiceListNode<TValue>.Choice(kvp.Key, kvp.Value)),
      Configuration);
  }
}