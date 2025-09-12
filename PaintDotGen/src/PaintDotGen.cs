using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using Color = LolK.PaintDotNet.Utils.Color;
using Math = LolK.PaintDotNet.Utils.Math;

[assembly: AssemblyTitle("Paint.GEN")]
[assembly: AssemblyDescription("Nothing yet")]
[assembly: AssemblyCompany("catiqueue")]
[assembly: AssemblyProduct("Paint.GEN")]
[assembly: AssemblyCopyright("Copyright © catiqueue 2020")]
[assembly: AssemblyCulture("en-US")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("0.3.0")]
[assembly: TargetPlatform("Windows7.0")]
[assembly: SupportedOSPlatform("Windows7.0")]

namespace LolK.PaintDotNet.Plugins.PaintDotGen {
  public class PluginSupportInfo : IPluginSupportInfo {
    private readonly Assembly _assembly = typeof(PluginSupportInfo).Assembly;
    public string Author => _assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()!.Copyright;
    public string Copyright => _assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()!.Description;
    public string DisplayName => _assembly.GetCustomAttribute<AssemblyProductAttribute>()!.Product;
    public Version Version => _assembly.GetName().Version;
    public Uri WebsiteUri => new("https://github.com/catiqueue");
    // ReSharper disable twice MemberCanBeMadeStatic.Global
#pragma warning disable CA1822
    public Image Icon => null; //Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQBAMAAADt3eJSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAwUExURQAAAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFulh5UAAAAJcEhZcwAADsEAAA7BAbiRa+0AAABFSURBVBjTLY2BDcAwDMLgA/j/2Zlmqaoi46hiHN3U9zo/qg8BzgJkaECzBlzQjFKoGKLgKjSLTmQcmu6XkDgvhNU+4lYf2ugD+FG4dkAAAAAASUVORK5CYII="), 0, 236), true);
    public string SubMenu => SubmenuNames.Render; // Do not forget to change
#pragma warning restore CA1822
    public CultureInfo Culture => _assembly.GetName().CultureInfo;
  }

  [PluginSupportInfo(typeof(PluginSupportInfo))]
  // ReSharper disable once UnusedType.Global
  public class Plugin : PropertyBasedEffect {
    private static readonly PluginSupportInfo Info = new();

    private static readonly Dictionary<char, ParameterExpression> Parameters = new() {
      { 'X', Expression.Parameter(typeof(float), "X") },
      { 'Y', Expression.Parameter(typeof(float), "Y") },
    };
    private static readonly Dictionary<string, ConstantExpression> Constants = new() {
      { "Pi", Expression.Constant(MathF.PI) },
      { "16f", Expression.Constant(16f) },
      { "2", Expression.Constant(2) },
      { "2f", Expression.Constant(2f) },
    };
    private static readonly Dictionary<string, MethodInfo> Cache = new() {
      { "Abs",   typeof(MathF).GetMethod("Abs") },
      { "Asin",  typeof(MathF).GetMethod("Asin") },
      { "Sin",   typeof(MathF).GetMethod("Sin") },
      { "Acos",  typeof(MathF).GetMethod("Acos") },
      { "Cos",   typeof(MathF).GetMethod("Cos") },
      { "Log",   typeof(MathF).GetMethod("Log", new[] { typeof(float), typeof(float) }) },
      { "Round", typeof(MathF).GetMethod("Round", new[] { typeof(float), typeof(int) }) },
    };

    private int _seed, _complexity;
    private Random _rng = new();
    private (Func<float, float, float> Func, (float Min, float Max) Bounds) _h, _s, _v;

    public Plugin() : base(Info.DisplayName, Info.Icon, Info.SubMenu, new EffectOptions { Flags = EffectFlags.Configurable }) { }

    private enum PropertyNames { Seed, Complexity }

    protected override PropertyCollection OnCreatePropertyCollection() {
      var properties = new Property[] {
        new Int32Property(PropertyNames.Seed, 0, int.MinValue, int.MaxValue),
        new Int32Property(PropertyNames.Complexity, 6, 2, 16),
      };
      
      return new PropertyCollection(properties);
    }

    protected override ControlInfo OnCreateConfigUI(PropertyCollection properties) {
      var configUi = CreateDefaultConfigUI(properties);

      configUi.SetPropertyControlValue(PropertyNames.Seed, ControlInfoPropertyNames.DisplayName, "Seed");
      configUi.SetPropertyControlValue(PropertyNames.Complexity, ControlInfoPropertyNames.DisplayName, "Complexity");

      return configUi;
    }

    protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection properties) {
      properties[ControlInfoPropertyNames.WindowHelpContentType].Value = WindowHelpContentType.PlainText;
      properties[ControlInfoPropertyNames.WindowHelpContent].Value = string.Format(Info.Culture, "{0}\n{1} v{2}\n{3}\nAll rights reserved.", Info.Copyright, Info.DisplayName, Info.Version, Info.Author);

      base.OnCustomizeConfigUIWindowProperties(properties);
    }

    /* */
    private bool ParametersChanged(PropertyBasedEffectConfigToken token) => 
      _seed != token.GetProperty<Int32Property>(PropertyNames.Seed).Value ||
      _complexity != token.GetProperty<Int32Property>(PropertyNames.Complexity).Value;

    /* */
    private void UpdateParameters(PropertyBasedEffectConfigToken token) {
      _seed = token.GetProperty<Int32Property>(PropertyNames.Seed).Value;
      _complexity = token.GetProperty<Int32Property>(PropertyNames.Complexity).Value;
    }

    /* */
    private void Regenerate() {
      _rng = new Random(_seed);

      _h.Func = GenerateExpression(_complexity).Compile();
      _s.Func = GenerateExpression(_complexity).Compile();
      _v.Func = GenerateExpression(_complexity).Compile();

      _h.Bounds = FindBounds(_h.Func);
      _s.Bounds = FindBounds(_s.Func);
      _v.Bounds = FindBounds(_v.Func);
    }

    protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs) {
      if(ParametersChanged(newToken)) {
        UpdateParameters(newToken); Regenerate();
      } else UpdateParameters(newToken);

      base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
    }

    protected override void OnRender(Rectangle[] rectangles, int startIndex, int length) {
      // ReSharper disable once EmptyEmbeddedStatement
      for (var i = startIndex; i < startIndex + length; Render(SrcArgs.Surface, DstArgs.Surface, rectangles[i++]));
    }

    // ReSharper disable once UnusedParameter.Local
    private void Render(Surface source, Surface destination, Rectangle drawingArea) {
      for (var y = drawingArea.Top; y < drawingArea.Bottom && !IsCancelRequested; y++) for (var x = drawingArea.Left; x < drawingArea.Right; x++) {
        destination[x, y] = Color.FromHSV(
          (int)(Math.Normalize(_h.Func(x, y), _h.Bounds.Min, _h.Bounds.Max) * 360),
          (int)(Math.Normalize(_s.Func(x, y), _s.Bounds.Min, _s.Bounds.Max) * 100),
          (int)(Math.Normalize(_v.Func(x, y), _v.Bounds.Min, _v.Bounds.Max) * 100));
      }
    }

    private Expression<Func<float, float, float>> GenerateExpression(int complexity) => Expression.Lambda<Func<float, float, float>>(GenerateExpression(complexity, complexity), Parameters['X'], Parameters['Y']);
    private Expression GenerateExpression(in int maxComplexity, int complexity) {
      var randomNumber = (float) _rng.NextDouble();
      var shouldEnd = randomNumber <= (maxComplexity - complexity) / (maxComplexity - 1f);
      if(shouldEnd) return randomNumber switch { // i dont care its not exhaustive 
        < 1f/3f => Parameters['X'],
        < 2f/3f => Parameters['Y'],
        < 3f/3f => Expression.Constant((float) _rng.Next(System.Math.Max(SrcArgs.Width, SrcArgs.Height))),
        _ => throw new ArgumentOutOfRangeException(nameof(randomNumber))
      };
      return randomNumber switch {
        <  1f/11f => Expression.Call(Cache["Abs"],   GenerateExpression(maxComplexity, complexity - 1)), // inner
        // <  2f/11f => Expression.Divide(Expression.Call(Cache["Asin"],  GenerateExpression(MaxComplexity, Complexity - 1)), Expression.Divide(Constants["Pi"], Constants["2f"])), // -1 - 1
        // <  2f/11f => Expression.Call(Cache["Asin"],  GenerateExpression(MaxComplexity, Complexity - 1)), // -pi/2 - pi/2
        <  3f/11f => Expression.Call(Cache["Sin"],   GenerateExpression(maxComplexity, complexity - 1)), // -1 - 1
        // <  4f/11f => Expression.Divide(Expression.Call(Cache["Acos"],  GenerateExpression(MaxComplexity, Complexity - 1)), Expression.Divide(Constants["Pi"], Constants["2f"])), // -1 - 1
        // <  4f/11f => Expression.Call(Cache["Acos"],  GenerateExpression(MaxComplexity, Complexity - 1)), // 0 - pi
        <  5f/11f => Expression.Call(Cache["Cos"],   GenerateExpression(maxComplexity, complexity - 1)), // -1 - 1
        <  6f/11f => Expression.Call(Cache["Log"],   Expression.Call(Cache["Abs"], GenerateExpression(maxComplexity, complexity - 1)), Constants["16f"]),
        <  7f/11f => Expression.Call(Cache["Round"], GenerateExpression(maxComplexity, complexity - 1), Constants["2"]),
        <  8f/11f => Expression.Add(GenerateExpression(maxComplexity, complexity - 1), GenerateExpression(maxComplexity, complexity - 1)),
        <  9f/11f => Expression.Subtract(GenerateExpression(maxComplexity, complexity - 1), GenerateExpression(maxComplexity, complexity - 1)),
        < 10f/11f => Expression.Multiply(GenerateExpression(maxComplexity, complexity - 1), GenerateExpression(maxComplexity, complexity - 1)),
        < 11f/11f => Expression.Divide(GenerateExpression(maxComplexity, complexity - 1), GenerateExpression(maxComplexity, complexity - 1)),
        _ => throw new ArgumentOutOfRangeException(nameof(randomNumber))
      };
    }
    private (float Min, float Max) FindBounds(Func<float, float, float> function) {
      (float Min, float Max) bounds = (float.MaxValue, float.MinValue);
      for(var y = 0; y < SrcArgs.Height; ++y) for(var x = 0; x < SrcArgs.Width; ++x) {
        var value = function(x, y);
        if(value < bounds.Min) bounds.Min = value;
        if(value > bounds.Max) bounds.Max = value;
      }
      return bounds;
    }
  }
}