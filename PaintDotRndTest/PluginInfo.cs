using catiqueue.PaintDotNet.Plugins.Common;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotRndTest;

internal sealed record PluginInfo : PluginInfoBase {
  public override string DisplayName => "Paint.RND (TEST)";
  public override string Author => "catiqueue";
  public override string Description => "Simple random effect for Paint.NET";
  public override string VersionString => "1.6.0";
  public override string SubMenu => SubmenuNames.Render;
  public override string WebsiteUriString => "https://github.com/catiqueue";
  public override string Base64Image => "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsEAAA7BAbiRa+0AAADbSURBVDhPfZONEYUwCINb5+jMDqJj6R59CSWVove+u9hAAf9L7z2rlVJ26II6BeiZa6HOtATQDhmheXoXa2bPY0o5sFpRxJs8mrB2GTDP/A8Nc9mVsJn3ZUmhIvlM2OfzsoezbMRYq7zw3F5xuBC3WisrEA5y/AVqbg7wgQM15lV7JOY3yzgqyMRGDdO6wdzmHG6oQWeRF/Qe37yCMySWwogGJU4eXq9RKBf3km82GXq9yohyqWZ+SCZwqIDIc8156PUpj8A/qizh/vtnmonxTOx3xmrQQx+/cy8/P+HuS6isW+4AAAAASUVORK5CYII=";
}