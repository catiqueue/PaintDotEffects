using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using PaintDotNet;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

internal sealed class PluginSupportInfo : IPluginSupportInfo {
  public string Author => Constants.Author;
  public string Copyright => Constants.Copyright;
  public string DisplayName => Constants.EffectName;
  public Version Version => new(Constants.Version);
  public Uri WebsiteUri => new(Constants.Website);
  
  public static CultureInfo Culture => new(Constants.Culture);
  public static Image Icon { get; } = Image.FromStream(new MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQBAMAAADt3eJSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJUExURf////8AAAAAAJqVApEAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAXSURBVBjTY2AQgEIhKIALIETopkZICADJXwaBQIZGVgAAAABJRU5ErkJggg=="), 0, 151), true);
  public static string SubMenu => SubmenuNames.Render;
}