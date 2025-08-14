using System;
using System.Drawing;

using System.Reflection;

using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

[assembly: AssemblyTitle("Paint.RND")]
[assembly: AssemblyDescription("Simple random effect for Paint.NET")]
[assembly: AssemblyCompany("catiqueue")]
[assembly: AssemblyProduct("Paint.RND")]
[assembly: AssemblyCopyright("Copyright © catiqueue 2021")]
[assembly: AssemblyCulture("en-US")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: AssemblyVersion("1.4.1")]
[assembly: System.Runtime.Versioning.TargetPlatform("Windows7.0")]
[assembly: System.Runtime.Versioning.SupportedOSPlatform("Windows7.0")]

namespace LolK.PaintDotNet.Plugins {
  public class PluginSupportInfo : IPluginSupportInfo {
    private readonly Assembly Assembly = typeof(PluginSupportInfo).Assembly;
    public string Author => Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
    public string Copyright => Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
    public string DisplayName => Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
    public Version Version => Assembly.GetName().Version;
    public Uri WebsiteUri => new Uri("https://github.com/catiqueue");
    public Image Icon => Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsEAAA7BAbiRa+0AAADbSURBVDhPfZONEYUwCINb5+jMDqJj6R59CSWVove+u9hAAf9L7z2rlVJ26II6BeiZa6HOtATQDhmheXoXa2bPY0o5sFpRxJs8mrB2GTDP/A8Nc9mVsJn3ZUmhIvlM2OfzsoezbMRYq7zw3F5xuBC3WisrEA5y/AVqbg7wgQM15lV7JOY3yzgqyMRGDdO6wdzmHG6oQWeRF/Qe37yCMySWwogGJU4eXq9RKBf3km82GXq9yohyqWZ+SCZwqIDIc8156PUpj8A/qizh/vtnmonxTOx3xmrQQx+/cy8/P+HuS6isW+4AAAAASUVORK5CYII="), 0, 326), true);
    public string SubMenu => SubmenuNames.Render; // IMPORTANT!
    public System.Globalization.CultureInfo Culture => Assembly.GetName().CultureInfo;
  }

  [PluginSupportInfo(typeof(PluginSupportInfo))]
  public class PaintDotRnd : PropertyBasedEffect {
    private static PluginSupportInfo Info = new PluginSupportInfo();

    Random RNG;
    byte[] RNGData;

    int Zoom = 1, 
        Precision = 2,
        Seed = -1, // important
        InstanceSeed = (int)(DateTime.UtcNow.Ticks / 10000000 - 946684800);

    public PaintDotRnd() : base(Info.DisplayName, Info.Icon, Info.SubMenu, new EffectOptions() { Flags = EffectFlags.Configurable }) { }

    private enum PropertyNames { Zoom, Precision, Seed }

    protected override PropertyCollection OnCreatePropertyCollection() {
      Property[] Properties = new Property[] {
        new Int32Property(PropertyNames.Zoom, 1, 1, 128),
        new Int32Property(PropertyNames.Precision, 2, 2, 256),
        new Int32Property(PropertyNames.Seed, 0, 0, int.MaxValue)
      };

      PropertyCollectionRule[] Rules = new PropertyCollectionRule[] { };

      return new PropertyCollection(Properties, Rules);
    }

    protected override ControlInfo OnCreateConfigUI(PropertyCollection Properties) {
      ControlInfo ConfigUI = CreateDefaultConfigUI(Properties);

      ConfigUI.SetPropertyControlValue(PropertyNames.Zoom, ControlInfoPropertyNames.DisplayName, "Zoom");

      ConfigUI.SetPropertyControlValue(PropertyNames.Precision, ControlInfoPropertyNames.DisplayName, "Precision");

      ConfigUI.SetPropertyControlValue(PropertyNames.Seed, ControlInfoPropertyNames.DisplayName, string.Empty);
      ConfigUI.SetPropertyControlType(PropertyNames.Seed, PropertyControlType.IncrementButton);
      ConfigUI.SetPropertyControlValue(PropertyNames.Seed, ControlInfoPropertyNames.ButtonText, "Reseed");

      return ConfigUI;
    }

    protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection Properties) {
      Properties[ControlInfoPropertyNames.WindowHelpContentType].Value = WindowHelpContentType.PlainText;
      Properties[ControlInfoPropertyNames.WindowHelpContent].Value = string.Format(Info.Culture, "{0}\n{1} v{2}\n{3}\nAll rights reserved.", Info.Copyright, Info.DisplayName, Info.Version, Info.Author);

      base.OnCustomizeConfigUIWindowProperties(Properties);
    }

    protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken NewToken, RenderArgs DstArgs, RenderArgs SrcArgs) {
      int NewSeed = NewToken.GetProperty<Int32Property>(PropertyNames.Seed).Value;      
      if(NewSeed != Seed) { 
        Seed = NewSeed;
        RNG = new(InstanceSeed ^ Seed);
        RNG.NextBytes(RNGData = new byte[SrcArgs.Width * SrcArgs.Height]);
      }

      Precision = NewToken.GetProperty<Int32Property>(PropertyNames.Precision).Value;
      Zoom = NewToken.GetProperty<Int32Property>(PropertyNames.Zoom).Value;

      base.OnSetRenderInfo(NewToken, DstArgs, SrcArgs);
    }

    protected override void OnRender(Rectangle[] Rectangles, int StartIndex, int Length) {
      for (int i = StartIndex; i < StartIndex + Length; ++i) Render(SrcArgs.Surface, DstArgs.Surface, Rectangles[i]);
    }

    private void Render(Surface Source, Surface Destination, Rectangle Rectangle) {
      for (int y = Rectangle.Top; y < Rectangle.Bottom && !IsCancelRequested; y++) for (int x = Rectangle.Left; x < Rectangle.Right; x++)
        Destination[x, y] = Utils.Color.FromGray((byte) Utils.Math.Precision(RNGData[(x / Zoom) + (y / Zoom) * Source.Width], byte.MaxValue, Precision));
    }
  }
}