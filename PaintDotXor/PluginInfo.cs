using catiqueue.PaintDotNet.Plugins.Common.FrameworkDependent;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXor;

internal sealed record PluginInfo : PluginInfoBase {
  public override string DisplayName => "Paint.XOR";
  public override string Author => "catiqueue";
  public override string? Description => "XOR pattern effect for Paint.NET";
  public override string VersionString => "1.5.0";
  public override string SubMenu => SubmenuNames.Render;
  public override string? WebsiteUriString => "https://github.com/catiqueue";
  public override string? Base64Image => "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsEAAA7BAbiRa+0AAABVSURBVDhPfYpBDsAwCMP4/6fZJtSGhs4+RAQnXjJzZ0Efcgv/9N53hVvvJ2Snmyl4Vyl65/V3e+e1X7e1W++87vytewreVQpy03o/udu5pg+5hT4RD2PJHvDN2VqUAAAAAElFTkSuQmCC";
}