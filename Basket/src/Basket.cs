using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Basket")]
[assembly: AssemblyDescription("Basket pattern effect for Paint.NET")]
[assembly: AssemblyCompany("catiqueue")]
[assembly: AssemblyProduct("Basket")]
[assembly: AssemblyCopyright("Copyright © catiqueue 2022")]
[assembly: AssemblyCulture("en-US")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.1.1")]
[assembly: System.Runtime.Versioning.TargetPlatform("Windows7.0")]
[assembly: System.Runtime.Versioning.SupportedOSPlatform("Windows7.0")]

namespace Basket {
  public class PluginSupportInfo : IPluginSupportInfo {
    private readonly Assembly assembly = typeof(PluginSupportInfo).Assembly;
    public string Author => assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
    public string Copyright => assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
    public string DisplayName => assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
    public Version Version => assembly.GetName().Version;
    public Uri WebsiteUri => new Uri("https://example.com");
    public Image Icon => Image.FromStream(new MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQBAMAAADt3eJSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJUExURf////8AAAAAAJqVApEAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAXSURBVBjTY2AQgEIhKIALIETopkZICADJXwaBQIZGVgAAAABJRU5ErkJggg=="), 0, 151), true);
    public string SubMenu => SubmenuNames.Render;
    public System.Globalization.CultureInfo Culture => assembly.GetName().CultureInfo;
  }

  [PluginSupportInfo(typeof(PluginSupportInfo))]
  public class Plugin : PropertyBasedEffect {
    private static PluginSupportInfo Info = new PluginSupportInfo();

    private ColorBgra backColor = ColorBgra.Black;
    private ColorBgra xColor = ColorBgra.Red;
    private ColorBgra yColor = ColorBgra.White;
    // private Random RNG;
    private int xSpacer = 1, ySpacer = 1;
    private int xSize = 1, ySize = 1;

    public Plugin() : base(Info.DisplayName, Info.Icon, Info.SubMenu, new EffectOptions() { Flags = EffectFlags.Configurable }) { }

    private enum PropertyNames { FirstColor, SecondColor, BackgroundColor, XSpacer, YSpacer, XSize, YSize }

    protected override PropertyCollection OnCreatePropertyCollection() {
      Property[] Properties = new Property[] {
        new Int32Property(PropertyNames.BackgroundColor, unchecked((int)ColorBgra.Black.Bgra), int.MinValue, int.MaxValue),
        new Int32Property(PropertyNames.FirstColor, unchecked((int)EnvironmentParameters.PrimaryColor.Bgra), int.MinValue, int.MaxValue),
        new Int32Property(PropertyNames.SecondColor, unchecked((int)ColorBgra.White.Bgra), int.MinValue, int.MaxValue),
        new Int32Property(PropertyNames.XSpacer, 1, 1, 255),
        new Int32Property(PropertyNames.YSpacer, 1, 1, 255),
        new Int32Property(PropertyNames.XSize, 1, 1, 255),
        new Int32Property(PropertyNames.YSize, 1, 1, 255)
      };

      return new PropertyCollection(Properties);
    }

    protected override ControlInfo OnCreateConfigUI(PropertyCollection Properties) {
      ControlInfo ConfigUI = CreateDefaultConfigUI(Properties);

      ConfigUI.SetPropertyControlValue(PropertyNames.FirstColor, ControlInfoPropertyNames.DisplayName, "First color");
      ConfigUI.SetPropertyControlType(PropertyNames.FirstColor, PropertyControlType.ColorWheel);
      ConfigUI.SetPropertyControlValue(PropertyNames.FirstColor, ControlInfoPropertyNames.ShowResetButton, false);

      ConfigUI.SetPropertyControlValue(PropertyNames.SecondColor, ControlInfoPropertyNames.DisplayName, "Second color");
      ConfigUI.SetPropertyControlType(PropertyNames.SecondColor, PropertyControlType.ColorWheel);
      ConfigUI.SetPropertyControlValue(PropertyNames.SecondColor, ControlInfoPropertyNames.ShowResetButton, false);

      ConfigUI.SetPropertyControlValue(PropertyNames.BackgroundColor, ControlInfoPropertyNames.DisplayName, "Background color");
      ConfigUI.SetPropertyControlType(PropertyNames.BackgroundColor, PropertyControlType.ColorWheel);
      ConfigUI.SetPropertyControlValue(PropertyNames.BackgroundColor, ControlInfoPropertyNames.ShowResetButton, false);

      ConfigUI.SetPropertyControlValue(PropertyNames.XSpacer, ControlInfoPropertyNames.DisplayName, "Vertical spacing");
      ConfigUI.SetPropertyControlValue(PropertyNames.YSpacer, ControlInfoPropertyNames.DisplayName, "Horizontal spacing");

      ConfigUI.SetPropertyControlValue(PropertyNames.XSize, ControlInfoPropertyNames.DisplayName, "V-lines size");
      ConfigUI.SetPropertyControlValue(PropertyNames.YSize, ControlInfoPropertyNames.DisplayName, "H-lines size");

      return ConfigUI;
    }

    protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection Properties) {
      Properties[ControlInfoPropertyNames.WindowHelpContentType].Value = WindowHelpContentType.PlainText;
      Properties[ControlInfoPropertyNames.WindowHelpContent].Value = string.Format(Info.Culture, "{0}\n{1} v{2}\n{3}\nAll rights reserved.", Info.Copyright, Info.DisplayName, Info.Version, Info.Author);

      base.OnCustomizeConfigUIWindowProperties(Properties);
    }

    protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken NewToken, RenderArgs DstArgs, RenderArgs SrcArgs) {
      xColor = ColorBgra.FromUInt32(unchecked((uint)NewToken.GetProperty<Int32Property>(PropertyNames.FirstColor).Value));
      yColor = ColorBgra.FromUInt32(unchecked((uint)NewToken.GetProperty<Int32Property>(PropertyNames.SecondColor).Value));
      backColor = ColorBgra.FromUInt32(unchecked((uint)NewToken.GetProperty<Int32Property>(PropertyNames.BackgroundColor).Value));
      xSpacer = Token.GetProperty<Int32Property>(PropertyNames.XSpacer).Value;
      ySpacer = Token.GetProperty<Int32Property>(PropertyNames.YSpacer).Value;
      xSize = Token.GetProperty<Int32Property>(PropertyNames.XSize).Value;
      ySize = Token.GetProperty<Int32Property>(PropertyNames.YSize).Value;
      base.OnSetRenderInfo(NewToken, DstArgs, SrcArgs);
    }

    protected override void OnRender(Rectangle[] Rectangles, int StartIndex, int Length) {
      for (int i = StartIndex; i < StartIndex + Length; Render(SrcArgs.Surface, DstArgs.Surface, Rectangles[i++]));
    }

    private void Render(Surface Source, Surface Destination, Rectangle Rectangle) {
      for (int y = Rectangle.Top; y < Rectangle.Bottom && !IsCancelRequested; y++) {
        for (int x = Rectangle.Left; x < Rectangle.Right; x++) {
          Destination[x, y] = Filter(x / xSize, y / ySize, xSpacer + 1, ySpacer + 1)
            ? IsJunction(x / xSize, y / ySize, xSpacer + 1, ySpacer + 1)
                ? IsLineEven(x / xSize, xSpacer + 1) ^ IsLineEven(y / ySize, ySpacer + 1)
                    ? xColor
                    : yColor
                : IsLineActive(y / ySize, ySpacer + 1)
                    ? yColor
                    : xColor
            : backColor;
        }
      }
    }

    private bool IsLineActive(int num, int spacer) => num % spacer == 0;
    private bool IsLineEven(int num, int spacer) => num % (spacer * 2) == 0;
    private bool Filter(int x, int y, int xSpacer, int ySpacer) => IsLineActive(x, xSpacer) || IsLineActive(y, ySpacer);
    private bool IsJunction(int x, int y, int xSpacer, int ySpacer) => IsLineActive(x, xSpacer) && IsLineActive(y, ySpacer);
  }
}