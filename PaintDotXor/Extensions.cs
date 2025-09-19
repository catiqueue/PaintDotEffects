using System;
using catiqueue.PaintDotNet.Plugins.Common.Data;
using catiqueue.PaintDotNet.Plugins.Common.Rendering;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXor;

internal static class Extensions {
  public static PropertyCollection GetProperties(this Plugin plugin) => new(
    properties: [
      new Int32Property(Plugin.PropertyNames.OffsetX, 0, 0, ushort.MaxValue),
      new Int32Property(Plugin.PropertyNames.OffsetY, 0, 0, ushort.MaxValue),
      new Int32Property(Plugin.PropertyNames.Zoom, 1, 1, 128),
      StaticListChoiceProperty.CreateForEnum<FilterChoice>(Plugin.PropertyNames.FilterMode, 0, false),
      new Int32Property(Plugin.PropertyNames.Divisor, 1, 1, 512),
      StaticListChoiceProperty.CreateForEnum<OperationChoice>(Plugin.PropertyNames.Operation, 0, false),
      new BooleanProperty(Plugin.PropertyNames.UseHSV, true),
      new ManagedColorProperty(Plugin.PropertyNames.Color, ManagedColor.Create(SrgbColors.Black))
    ],
    rules: [
      new ReadOnlyBoundToValueRule<object, StaticListChoiceProperty>(Plugin.PropertyNames.Divisor, Plugin.PropertyNames.FilterMode, FilterChoice.IsPrime, false),
      new ReadOnlyBoundToBooleanRule(Plugin.PropertyNames.Color, Plugin.PropertyNames.UseHSV, false)
    ]);
    
  public static ControlInfo GetConfigUI(this Plugin plugin, PropertyCollection properties) {
    ControlInfo configUi = PropertyBasedBitmapEffect.CreateDefaultConfigUI(properties);

    configUi.SetPropertyControlValue(Plugin.PropertyNames.OffsetX, ControlInfoPropertyNames.DisplayName, "X offset");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.OffsetY, ControlInfoPropertyNames.DisplayName, "Y offset");

    configUi.SetPropertyControlValue(Plugin.PropertyNames.Zoom, ControlInfoPropertyNames.DisplayName, "Zoom");

    configUi.SetPropertyControlValue(Plugin.PropertyNames.FilterMode, ControlInfoPropertyNames.DisplayName, "Filter mode");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.FilterMode)!.SetValueDisplayName(FilterChoice.IsPrime, "Is prime?");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.FilterMode)!.SetValueDisplayName(FilterChoice.IsDivisible, "Is divisible?");

    configUi.SetPropertyControlValue(Plugin.PropertyNames.Divisor, ControlInfoPropertyNames.DisplayName, "Divisor");

    configUi.SetPropertyControlValue(Plugin.PropertyNames.Operation, ControlInfoPropertyNames.DisplayName, "Operation");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.Operation)!.SetValueDisplayName(OperationChoice.XOR, "XOR");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.Operation)!.SetValueDisplayName(OperationChoice.AND, "AND");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.Operation)!.SetValueDisplayName(OperationChoice.OR, "OR");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.Operation)!.SetValueDisplayName(OperationChoice.BitReversedXOR, "Bit reversed XOR");
    configUi.FindControlForPropertyName(Plugin.PropertyNames.Operation)!.SetValueDisplayName(OperationChoice.GrayCodeXOR, "Gray encoded XOR");

    configUi.SetPropertyControlValue(Plugin.PropertyNames.UseHSV, ControlInfoPropertyNames.DisplayName, "");
    configUi.SetPropertyControlValue(Plugin.PropertyNames.UseHSV, ControlInfoPropertyNames.Description, "Use HSV conversion");

    configUi.SetPropertyControlValue(Plugin.PropertyNames.Color, ControlInfoPropertyNames.DisplayName, "Pattern color");
    configUi.SetPropertyControlType(Plugin.PropertyNames.Color, PropertyControlType.ColorWheel);
    configUi.SetPropertyControlValue(Plugin.PropertyNames.Color, ControlInfoPropertyNames.ShowResetButton, false);

    return configUi;
  }
  
  internal static Settings ToSettings(this PropertyBasedEffectConfigToken token) => new() {
    Camera = new Camera(
      Offset: new Vector<int>(
        token.GetProperty<Int32Property>(Plugin.PropertyNames.OffsetX)!.Value,
        token.GetProperty<Int32Property>(Plugin.PropertyNames.OffsetY)!.Value),
      Zoom: token.GetProperty<Int32Property>(Plugin.PropertyNames.Zoom)!.Value
    ),
    Operation = OperationFactory.FromChoice(
      (OperationChoice)token.GetProperty<StaticListChoiceProperty>(Plugin.PropertyNames.Operation)!.Value),
    Filter = (FilterChoice)token.GetProperty<StaticListChoiceProperty>(Plugin.PropertyNames.FilterMode)!.Value switch {
      FilterChoice.IsDivisible => FilterFactory.DivisibleBy(
        token.GetProperty<Int32Property>(Plugin.PropertyNames.Divisor)!.Value),
      FilterChoice.IsPrime => FilterFactory.IsPrime,
      _ => throw new ArgumentOutOfRangeException(nameof(token), "Unknown filter choice!")
    },
    Painter = token.GetProperty<BooleanProperty>(Plugin.PropertyNames.UseHSV)!.Value
      ? PainterFactory.SineHsvPainter
      : PainterFactory.ConstantColorPainter(token.GetProperty<ManagedColorProperty>(Plugin.PropertyNames.Color)!.Value)
  };
}