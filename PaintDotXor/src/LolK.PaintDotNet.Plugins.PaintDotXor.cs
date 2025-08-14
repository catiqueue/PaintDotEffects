using System;
using System.Drawing;

using System.Reflection;

using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

[assembly: AssemblyTitle("Paint.XOR")]
[assembly: AssemblyDescription("XOR pattern effect for Paint.NET")]
[assembly: AssemblyCompany("catiqueue")]
[assembly: AssemblyProduct("Paint.XOR")]
[assembly: AssemblyCopyright("Copyright © catiqueue 2021")]
[assembly: AssemblyCulture("en-US")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: AssemblyVersion("1.3.0")]
[assembly: System.Runtime.Versioning.TargetPlatform("Windows7.0")]
[assembly: System.Runtime.Versioning.SupportedOSPlatform("Windows7.0")]

namespace LolK.PaintDotNet.Plugins {
  public class PluginSupportInfo : IPluginSupportInfo {
    private readonly Assembly assembly = typeof(PluginSupportInfo).Assembly;
    public string Author => assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
    public string Copyright => assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
    public string DisplayName => assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
    public Version Version => assembly.GetName().Version;
    public Uri WebsiteUri => new Uri("https://github.com/catiqueue");
    // YEP. I'm an asshole. I've hardcoded a picture.
    public Image Icon => Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsEAAA7BAbiRa+0AAABVSURBVDhPfYpBDsAwCMP4/6fZJtSGhs4+RAQnXjJzZ0Efcgv/9N53hVvvJ2Snmyl4Vyl65/V3e+e1X7e1W++87vytewreVQpy03o/udu5pg+5hT4RD2PJHvDN2VqUAAAAAElFTkSuQmCC"), 0, 192), true);
    public string SubMenu => SubmenuNames.Render;
    public System.Globalization.CultureInfo Culture => assembly.GetName().CultureInfo;
  }

  [PluginSupportInfo(typeof(PluginSupportInfo))]
  public class PaintDotXor : PropertyBasedEffect {
    private static PluginSupportInfo Info = new PluginSupportInfo();
    private int OffsetX = 0, OffsetY = 0;
    private int Zoom = 1;
    private byte CheckMode = 0, BitMode = 0;
    private int Divisor = 1;
    private bool UseHSV = true;
    private ColorBgra Color = ColorBgra.Black;
    
    public PaintDotXor() : base(Info.DisplayName, Info.Icon, Info.SubMenu, new EffectOptions() { Flags = EffectFlags.Configurable }) { }

    private enum PropertyNames { OffsetX, OffsetY, Zoom, CheckMode, Divisor, BitMode, UseHSV, Color }
    private enum CheckModeOptions { IsPrime, IsDivisible }
    private enum BitModeOptions { XOR, AND, OR } 

    protected override PropertyCollection OnCreatePropertyCollection() {
      Property[] Properties = new Property[] {
        new Int32Property(PropertyNames.OffsetX, 0, 0, ushort.MaxValue),
        new Int32Property(PropertyNames.OffsetY, 0, 0, ushort.MaxValue),
        new Int32Property(PropertyNames.Zoom, 1, 1, 128), // NEW
        StaticListChoiceProperty.CreateForEnum<CheckModeOptions>(PropertyNames.CheckMode, 0, false),
        new Int32Property(PropertyNames.Divisor, 1, 1, 512),
        StaticListChoiceProperty.CreateForEnum<BitModeOptions>(PropertyNames.BitMode, 0, false),
        new BooleanProperty(PropertyNames.UseHSV, true),
        new Int32Property(PropertyNames.Color, unchecked((int)EnvironmentParameters.PrimaryColor.Bgra), int.MinValue, int.MaxValue)
      };

      PropertyCollectionRule[] Rules = new PropertyCollectionRule[] {
        new ReadOnlyBoundToValueRule<object,StaticListChoiceProperty>(PropertyNames.Divisor, PropertyNames.CheckMode, CheckModeOptions.IsPrime, false),
        new ReadOnlyBoundToBooleanRule(PropertyNames.Color, PropertyNames.UseHSV, false)
      };

      return new PropertyCollection(Properties, Rules);
    }

    protected override ControlInfo OnCreateConfigUI(PropertyCollection Properties) {
      ControlInfo ConfigUI = CreateDefaultConfigUI(Properties);

      ConfigUI.SetPropertyControlValue(PropertyNames.OffsetX, ControlInfoPropertyNames.DisplayName, "X offset");
      ConfigUI.SetPropertyControlValue(PropertyNames.OffsetY, ControlInfoPropertyNames.DisplayName, "Y offset");

      ConfigUI.SetPropertyControlValue(PropertyNames.Zoom, ControlInfoPropertyNames.DisplayName, "Zoom");

      ConfigUI.SetPropertyControlValue(PropertyNames.CheckMode, ControlInfoPropertyNames.DisplayName, "Check mode");
      ConfigUI.FindControlForPropertyName(PropertyNames.CheckMode).SetValueDisplayName(CheckModeOptions.IsPrime, "Is prime?");
      ConfigUI.FindControlForPropertyName(PropertyNames.CheckMode).SetValueDisplayName(CheckModeOptions.IsDivisible, "Is divisible?");

      ConfigUI.SetPropertyControlValue(PropertyNames.Divisor, ControlInfoPropertyNames.DisplayName, "Divider");

      ConfigUI.SetPropertyControlValue(PropertyNames.BitMode, ControlInfoPropertyNames.DisplayName, "Bit mode");
      ConfigUI.FindControlForPropertyName(PropertyNames.BitMode).SetValueDisplayName(BitModeOptions.XOR, "XOR");
      ConfigUI.FindControlForPropertyName(PropertyNames.BitMode).SetValueDisplayName(BitModeOptions.AND, "AND");
      ConfigUI.FindControlForPropertyName(PropertyNames.BitMode).SetValueDisplayName(BitModeOptions.OR, "OR");

      ConfigUI.SetPropertyControlValue(PropertyNames.UseHSV, ControlInfoPropertyNames.DisplayName, "");
      ConfigUI.SetPropertyControlValue(PropertyNames.UseHSV, ControlInfoPropertyNames.Description, "Use HSV conversion");

      ConfigUI.SetPropertyControlValue(PropertyNames.Color, ControlInfoPropertyNames.DisplayName, "Pattern color");
      ConfigUI.SetPropertyControlType(PropertyNames.Color, PropertyControlType.ColorWheel);
      ConfigUI.SetPropertyControlValue(PropertyNames.Color, ControlInfoPropertyNames.ShowResetButton, false);

      return ConfigUI;
    }

    protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection Properties) {
      Properties[ControlInfoPropertyNames.WindowHelpContentType].Value = WindowHelpContentType.PlainText;
      Properties[ControlInfoPropertyNames.WindowHelpContent].Value = string.Format(Info.Culture, "{0}\n{1} v{2}\n{3}\nAll rights reserved.", Info.Copyright, Info.DisplayName, Info.Version, Info.Author);

      base.OnCustomizeConfigUIWindowProperties(Properties);
    }

    protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken NewToken, RenderArgs DstArgs, RenderArgs SrcArgs) {
      OffsetX = NewToken.GetProperty<Int32Property>(PropertyNames.OffsetX).Value;
      OffsetY = NewToken.GetProperty<Int32Property>(PropertyNames.OffsetY).Value;
      Zoom = NewToken.GetProperty<Int32Property>(PropertyNames.Zoom).Value;
      CheckMode = (byte)((int)NewToken.GetProperty<StaticListChoiceProperty>(PropertyNames.CheckMode).Value);
      Divisor = NewToken.GetProperty<Int32Property>(PropertyNames.Divisor).Value;
      BitMode = (byte)((int)NewToken.GetProperty<StaticListChoiceProperty>(PropertyNames.BitMode).Value);
      UseHSV = NewToken.GetProperty<BooleanProperty>(PropertyNames.UseHSV).Value;
      Color = ColorBgra.FromUInt32(unchecked((uint)NewToken.GetProperty<Int32Property>(PropertyNames.Color).Value));
      base.OnSetRenderInfo(NewToken, DstArgs, SrcArgs);
    }

    protected override void OnRender(Rectangle[] Rectangles, int StartIndex, int Length) {
      for (int i = StartIndex; i < StartIndex + Length; Render(SrcArgs.Surface, DstArgs.Surface, Rectangles[i++]));
    }

    private void Render(Surface Source, Surface Destination, Rectangle Rectangle) {
      int Magic; 

      for (int y = Rectangle.Top; y < Rectangle.Bottom && !IsCancelRequested; y++) for (int x = Rectangle.Left; x < Rectangle.Right; x++) {
        Magic = (BitModeOptions) BitMode switch {
          BitModeOptions.XOR => (x / Zoom + OffsetX) ^ (y / Zoom + OffsetY),
          BitModeOptions.AND => (x / Zoom + OffsetX) & (y / Zoom + OffsetY),
          BitModeOptions.OR  => (x / Zoom + OffsetX) | (y / Zoom + OffsetY)
        };

        if ((CheckModeOptions) CheckMode switch { CheckModeOptions.IsPrime => Utils.Math.IsPrime(Magic), CheckModeOptions.IsDivisible => Utils.Math.IsDivisible(Magic, Divisor)}) // Checked
          if (UseHSV) 
            Destination[x, y] = Utils.Color.FromHSV((int) (Math.Sin(Magic) * 180 + 180), 100, (int) (Math.Cos(Magic) * 50 + 50));
          else // !HSV
            Destination[x, y] = Color;
        else // !Checked
          Destination[x, y] = Source[x, y];
      }
    }
  }
}