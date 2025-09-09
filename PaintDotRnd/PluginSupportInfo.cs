using System;
using System.Drawing;
using System.Globalization;
using PaintDotNet;
using PaintDotNet.Effects;

namespace catiqueue.PaintDotNet.Plugins.PaintDotRnd;

internal sealed class PluginSupportInfo : IPluginSupportInfo {
  // These should be also static, but oh well, we're implementing an interface.
  // I wonder if there's a way to set these up without the attribute.
  public string Author => Constants.Author;
  public string Copyright => Constants.Copyright;
  public string DisplayName => Constants.EffectName;
  public Version Version { get; } = new(Constants.Version);
  public Uri WebsiteUri { get; } = new(Constants.Website);
  
  public static CultureInfo Culture { get; } = new(Constants.Culture);
  // Oops. The stream is not disposed. Who cares in this case?
  public Image Icon { get; } = Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String(Constants.Base64Image), 0, Constants.Base64ImageStringLength), true);
  public static string SubMenu => SubmenuNames.Render;
}