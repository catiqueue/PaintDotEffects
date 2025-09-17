using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotGen;

internal sealed record PluginInfo : PluginInfoBase {
  public override string DisplayName => "PaintDotGen";
  public override string Author => "catiqueue";
  public override string? Description => "Random mathematical image generator plugin for Paint.NET";
  public override string VersionString => "1.0.1";
  public override string SubMenu => SubmenuNames.Render;
  public override string? WebsiteUriString => "https://github.com/catiqueue";
  public override string? Base64Image => "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQBAMAAADt3eJSAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAwUExURQAAAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFulh5UAAAAJcEhZcwAADsEAAA7BAbiRa+0AAABFSURBVBjTLY2BDcAwDMLgA/j/2Zlmqaoi46hiHN3U9zo/qg8BzgJkaECzBlzQjFKoGKLgKjSLTmQcmu6XkDgvhNU+4lYf2ugD+FG4dkAAAAAASUVORK5CYII=";
}