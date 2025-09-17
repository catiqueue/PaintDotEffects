using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotBasket;

internal sealed record PluginInfo : PluginInfoBase {
  public override string DisplayName => "PaintDotBasket";
  public override string Author => "catiqueue";
  public override string? Description => "Basket pattern effect for Paint.NET";
  public override string VersionString => "1.2.2";
  public override string SubMenu => SubmenuNames.Render;
  public override string? WebsiteUriString => "https://github.com/catiqueue";
  public override string? Base64Image => "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQBAMAAADt3eJSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJUExURf////8AAAAAAJqVApEAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAXSURBVBjTY2AQgEIhKIALIETopkZICADJXwaBQIZGVgAAAABJRU5ErkJggg==";
}