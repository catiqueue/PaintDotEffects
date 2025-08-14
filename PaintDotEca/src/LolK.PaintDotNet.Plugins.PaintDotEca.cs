using System;
using System.Collections;
using System.Drawing;

using System.Reflection;

using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

[assembly: AssemblyTitle("Paint.ECA")]
[assembly: AssemblyDescription("Elementary Cellular Automaton pattern effect for Paint.NET")]
[assembly: AssemblyCompany("catiqueue")]
[assembly: AssemblyProduct("Paint.ECA")]
[assembly: AssemblyCopyright("Copyright © catiqueue 2021")]
[assembly: AssemblyCulture("en-US")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: AssemblyVersion("1.5.0")]

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
    public Image Icon => Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQBAMAAADt3eJSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAwUExURQAAAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFulh5UAAAAJcEhZcwAADsEAAA7BAbiRa+0AAABFSURBVBjTLY2BDcAwDMLgA/j/2Zlmqaoi46hiHN3U9zo/qg8BzgJkaECzBlzQjFKoGKLgKjSLTmQcmu6XkDgvhNU+4lYf2ugD+FG4dkAAAAAASUVORK5CYII="), 0, 236), true);
    public string SubMenu => SubmenuNames.Render;
    public System.Globalization.CultureInfo Culture => assembly.GetName().CultureInfo;
  }

  [PluginSupportInfo(typeof(PluginSupportInfo))]
  public class PaintDotECA : PropertyBasedEffect {
    private static readonly PluginSupportInfo Info = new PluginSupportInfo();

    private int MaxWidth;
    private int Generations;
    private byte Rule;
    private BoundsHandlingOptions BoundsHandling;
    private int Zoom;
    private bool UseRtC;
    private ColorBgra Color;

    // private BitArray ECAData;
    private bool[,] ECAData;

    private enum PropertyNames { MaxWidth, Generations, Rule, BoundsHandling, Zoom, UseRTC, Color }

    private enum BoundsHandlingOptions { Wrap, WrapInvert, SetZero, SetOne }

    public PaintDotECA() : base(Info.DisplayName, Info.Icon, Info.SubMenu, new EffectOptions { Flags = EffectFlags.Configurable }) { }

    protected override PropertyCollection OnCreatePropertyCollection() {
      Property[] Properties = new Property[] {
        new Int32Property(PropertyNames.Rule, 30, 0, 255),
        new Int32Property(PropertyNames.MaxWidth, 4096, 4, 4096),
        new Int32Property(PropertyNames.Generations, 256, 4, 4096),
        StaticListChoiceProperty.CreateForEnum<BoundsHandlingOptions>(PropertyNames.BoundsHandling, 0, false),
        new Int32Property(PropertyNames.Zoom, 1, 1, 128),
        new BooleanProperty(PropertyNames.UseRTC, true),
        new Int32Property(PropertyNames.Color, unchecked((int)EnvironmentParameters.PrimaryColor.Bgra), int.MinValue, int.MaxValue)
      };

      PropertyCollectionRule[] Rules = new PropertyCollectionRule[] {
        new ReadOnlyBoundToBooleanRule(PropertyNames.Color, PropertyNames.UseRTC, false)
      };

      return new PropertyCollection(Properties, Rules);
    }

    protected override ControlInfo OnCreateConfigUI(PropertyCollection Properties) {
      ControlInfo ConfigUI = CreateDefaultConfigUI(Properties);

      ConfigUI.SetPropertyControlValue(PropertyNames.Rule, ControlInfoPropertyNames.DisplayName, "Rule");
      ConfigUI.SetPropertyControlValue(PropertyNames.MaxWidth, ControlInfoPropertyNames.DisplayName, "Max Width");
      ConfigUI.SetPropertyControlValue(PropertyNames.Generations, ControlInfoPropertyNames.DisplayName, "Height (№ of Generations)");
      ConfigUI.SetPropertyControlValue(PropertyNames.BoundsHandling, ControlInfoPropertyNames.DisplayName, "Bounds Handling Mode");
      ConfigUI.FindControlForPropertyName(PropertyNames.BoundsHandling).SetValueDisplayName(BoundsHandlingOptions.Wrap, "Wrap");
      ConfigUI.FindControlForPropertyName(PropertyNames.BoundsHandling).SetValueDisplayName(BoundsHandlingOptions.WrapInvert, "Invert Wrap");
      ConfigUI.FindControlForPropertyName(PropertyNames.BoundsHandling).SetValueDisplayName(BoundsHandlingOptions.SetZero, "Set False");
      ConfigUI.FindControlForPropertyName(PropertyNames.BoundsHandling).SetValueDisplayName(BoundsHandlingOptions.SetOne, "Set True");
      ConfigUI.SetPropertyControlValue(PropertyNames.Zoom, ControlInfoPropertyNames.DisplayName, "Zoom");
      ConfigUI.SetPropertyControlValue(PropertyNames.UseRTC, ControlInfoPropertyNames.Description, "Color by rule");
      ConfigUI.SetPropertyControlValue(PropertyNames.UseRTC, ControlInfoPropertyNames.DisplayName, "");
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

    /* */ bool ParametersChanged(PropertyBasedEffectConfigToken Token) => 
      Token.GetProperty<Int32Property>(PropertyNames.MaxWidth).Value != MaxWidth ||
      Token.GetProperty<Int32Property>(PropertyNames.Generations).Value != Generations ||
      (byte)Token.GetProperty<Int32Property>(PropertyNames.Rule).Value != Rule ||
      (BoundsHandlingOptions)Token.GetProperty<StaticListChoiceProperty>(PropertyNames.BoundsHandling).Value != BoundsHandling;

    /* */ void UpdateParameters(PropertyBasedEffectConfigToken Token) {
      MaxWidth = Token.GetProperty<Int32Property>(PropertyNames.MaxWidth).Value;
      Generations = Token.GetProperty<Int32Property>(PropertyNames.Generations).Value;
      Rule = (byte) Token.GetProperty<Int32Property>(PropertyNames.Rule).Value;
      BoundsHandling = (BoundsHandlingOptions) Token.GetProperty<StaticListChoiceProperty>(PropertyNames.BoundsHandling).Value;
      Zoom = Token.GetProperty<Int32Property>(PropertyNames.Zoom).Value;
      UseRtC = Token.GetProperty<BooleanProperty>(PropertyNames.UseRTC).Value;
      Color = ColorBgra.FromUInt32(unchecked((uint)Token.GetProperty<Int32Property>(PropertyNames.Color).Value));
    }

    /* */ void Regenerate() {
      bool HandledBound;

      ECAData = new bool[MaxWidth, Generations];
      ECAData[MaxWidth / 2, 0] = (MaxWidth % 2 == 0) ? ECAData[MaxWidth / 2 - 1, 0] = true : true;

      for(int y = 0; y < Generations - 1; ++y) { // read from y, write to y + 1;
        // process Left bound: x == 0;
        HandledBound = HandleBound(ECAData[1, y]);
        ECAData[0, y + 1] = GetChild(HandledBound, ECAData[0, y], ECAData[1, y]);

        for(int x = 1; x < MaxWidth - 1; ++x) ECAData[x, y + 1] = GetChild(ECAData[x - 1, y], ECAData[x, y], ECAData[x + 1, y]);

        // process Right bound: x == MaxWidth - 1;
        HandledBound = HandleBound(ECAData[MaxWidth - 2, y]);
        ECAData[MaxWidth - 1, y + 1] = GetChild(ECAData[MaxWidth - 2, y], ECAData[MaxWidth - 1, y], HandledBound);
      }
    }

    protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken NewToken, RenderArgs DstArgs, RenderArgs SrcArgs) {
      if(ParametersChanged(NewToken)) {
        UpdateParameters(NewToken); Regenerate();
      } else UpdateParameters(NewToken);

      base.OnSetRenderInfo(NewToken, DstArgs, SrcArgs);
    }

    protected override void OnRender(Rectangle[] renderRects, int startIndex, int length) {
      for (int i = startIndex; i < startIndex + length; ++i) Render(renderRects[i]);
    }

    private void Render(Rectangle DrawingArea) {
      Point Offset = new((SrcArgs.Width - MaxWidth * Zoom) / 2, (SrcArgs.Height - Generations * Zoom) / 2);
      Point Min = new(Math.Max(Offset.X, 0), Math.Max(Offset.Y, 0));
      Point Max = new(Math.Min(SrcArgs.Width - Offset.X, SrcArgs.Width), Math.Min(SrcArgs.Height - Offset.Y, SrcArgs.Height));
      Rectangle Canvas = new(Min, new(Max.X - Min.X, Max.Y - Min.Y));
      Rectangle ECA = new(new(), new(MaxWidth, Generations));
      Point Actual = new();

      for (int Y = DrawingArea.Top; Y < DrawingArea.Bottom && !IsCancelRequested; Y++) for (int X = DrawingArea.Left; X < DrawingArea.Right; X++) {
        Actual.X = (X - Offset.X) / Zoom; Actual.Y = (Y - Offset.Y) / Zoom; // i dont care its not efficient
        // bool checks() => Actual.X >= 0 && Actual.X < MaxWidth && Actual.Y >= 0 && Actual.Y < Generations && X >= Min.X && X < Max.X && Y >= Min.Y && Y < Max.Y;
        DstArgs.Surface[X, Y] = ECA.Contains(Actual) && Canvas.Contains(X, Y) && ECAData[Actual.X, Actual.Y] ? (UseRtC ? GetChildColor(Actual.X, Actual.Y) : Color) : SrcArgs.Surface[X, Y];
      }
    }

    [Obsolete]
    private void OldRender(Surface dst, Surface src, Rectangle rect) {
      int xOffset = (src.Bounds.Width - MaxWidth) / 2, yOffset = (src.Bounds.Height - Generations) / 2;

      for (int y = rect.Top; y < rect.Bottom && !IsCancelRequested; y++) {
        for (int x = rect.Left; x < rect.Right; x++) { // бл*н ты д*стал 
          Rectangle r = new Rectangle(x - xOffset, y - yOffset, src.Bounds.Width - xOffset, src.Bounds.Height - yOffset);
          if (r.Y * MaxWidth + r.X < ECAData.Length && r.Y >= 0 && r.X >= 0 && y < r.Height && x < r.Width && ECAData[r.X, r.Y])
            if (UseRtC) dst[x, y] = GetChildColor(r.X, r.Y);
            else dst[x, y] = Color;
          else dst[x, y] = src[x, y];
        }
      }
    }

    private (bool Left, bool Center, bool Right) GetParents(int x, int y) {
      if(x < 0 || x >= MaxWidth || y < 1 || y > Generations) return (false, false, false);
      return x == 0 ? (HandleBound(ECAData[x + 1, y - 1]), ECAData[x, y - 1], ECAData[x + 1, y - 1]) 
                    : x == MaxWidth - 1 ? (ECAData[x - 1, y - 1], ECAData[x, y - 1], HandleBound(ECAData[x - 1, y - 1]))
                    : (ECAData[x - 1, y - 1], ECAData[x, y - 1], ECAData[x + 1, y - 1]);
    }

    private bool GetChild(bool Left, bool Center, bool Right) => (Left, Center, Right) switch {
      {Left: true,  Center: true,  Right: true}  => GetBit(Rule, 7),
      {Left: true,  Center: true,  Right: false} => GetBit(Rule, 6),
      {Left: true,  Center: false, Right: true}  => GetBit(Rule, 5),
      {Left: true,  Center: false, Right: false} => GetBit(Rule, 4),
      {Left: false, Center: true,  Right: true}  => GetBit(Rule, 3),
      {Left: false, Center: true,  Right: false} => GetBit(Rule, 2),
      {Left: false, Center: false, Right: true}  => GetBit(Rule, 1),
      {Left: false, Center: false, Right: false} => GetBit(Rule, 0)
    };

    private bool HandleBound(bool Insider) => BoundsHandling switch {
      BoundsHandlingOptions.Wrap => Insider,
      BoundsHandlingOptions.WrapInvert => !Insider,
      BoundsHandlingOptions.SetOne => true,
      BoundsHandlingOptions.SetZero => false,
      _ => false // all other cases (there are none, but vscode warns about them)
    };

    private bool GetBit(byte rule, byte bit) => ((rule >> bit) & 1) != 0;

    private ColorBgra GetChildColor(int x, int y) {
      var parents = GetParents(x, y);
      return GetChildColor(parents.Left, parents.Center, parents.Right);
    }

    private ColorBgra GetChildColor(bool Left, bool Center, bool Right) => (Left,Center,Right) switch {
      {Left: true,  Center: true,  Right: true}  => ColorBgra.FromColor(new HsvColor(0, 100, 100).ToColor()),
      {Left: true,  Center: true,  Right: false} => ColorBgra.FromColor(new HsvColor(45, 100, 100).ToColor()),
      {Left: true,  Center: false, Right: true}  => ColorBgra.FromColor(new HsvColor(90, 100, 100).ToColor()),
      {Left: true,  Center: false, Right: false} => ColorBgra.FromColor(new HsvColor(135, 100, 100).ToColor()),
      {Left: false, Center: true,  Right: true}  => ColorBgra.FromColor(new HsvColor(180, 100, 100).ToColor()),
      {Left: false, Center: true,  Right: false} => ColorBgra.FromColor(new HsvColor(225, 100, 100).ToColor()),
      {Left: false, Center: false, Right: true}  => ColorBgra.FromColor(new HsvColor(270, 100, 100).ToColor()),
      {Left: false, Center: false, Right: false} => ColorBgra.FromColor(new HsvColor(315, 100, 100).ToColor())
    };
  }
}