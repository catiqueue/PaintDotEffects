using System.Collections.Generic;
using System.Linq;
using catiqueue.PaintDotNet.Plugins.Common.Exceptions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Building.Base;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Building.Values;

public sealed class UiEnumListBuilder<TSettings, TParent, TValue> 
  : UiValueBuilderBase<TSettings, TParent, UiEnumListBuilder<TSettings, TParent, TValue>, ChoiceListNode<TValue>, TValue> 
  where TSettings : class 
  where TValue : notnull 
{
  private readonly Dictionary<TValue, string> _choices = []; 
  
  internal UiEnumListBuilder(PluginUiBehaviorBuilder<TSettings> root, TParent parent) : base(root, parent) { }
  
  public UiEnumListBuilder<TSettings, TParent, TValue> WithChoice(TValue value, string name) {
    _choices[value] = name;
    return this;
  }
  
  public UiEnumListBuilder<TSettings, TParent, TValue> WithChoice(TValue value) 
    => WithChoice(value, value.ToString() ?? string.Empty);

  protected override ChoiceListNode<TValue> Build() {
    return new ChoiceListNode<TValue>(
      Name,
      _choices.FirstOrDefault() is var choice ? choice.Key : throw new IncompleteDefinitionException(nameof(UiEnumListBuilder<TSettings, TParent, TValue>), "Choices"),
      _choices.Select(kvp => new Choice<TValue>(kvp.Key, kvp.Value)),
      Configuration);
  }
}