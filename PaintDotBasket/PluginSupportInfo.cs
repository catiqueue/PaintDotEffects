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
  public static Image Icon { get; } = Image.FromStream(new MemoryStream(Convert.FromBase64String(Constants.Base64Image), 0, Constants.Base64ImageStringLength), true);
  public static string SubMenu => SubmenuNames.Render;
}