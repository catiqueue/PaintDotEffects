using System.Collections.Generic;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;

public sealed class ColorNode
(
  string? name, 
  ManagedColor? defaultValue, 
  IEnumerable<PropertyConfigEntry> configuration
) 
  : ValueNodeBase<ManagedColor>(name ?? $"ColorWheel_{_nameCounter++}", defaultValue ?? ManagedColor.Create(SrgbColors.Black), configuration, PropertyControlType.ColorWheel) 
{
  private static int _nameCounter = 0;
  protected override Property ToProperty() => new ManagedColorProperty(Name, DefaultValue);
}