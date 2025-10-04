using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using PaintDotNet;

namespace catiqueue.PaintDotNet.Plugins.Common;

public abstract record PluginInfoBase : IPluginSupportInfo {
  public abstract string DisplayName { get; }
  public abstract string Author { get; }
  public abstract string VersionString { get; }

  public virtual string? SubMenu => null;
  public virtual string? Description => null;
  public virtual string? WebsiteUriString => null;
  public virtual string CultureString => "en-US";
  public virtual string? Copyright => $"Copyright © {Author} {DateTime.Now.Year}";
  public virtual string? Base64Image => null;

  public Version Version => Version.Parse(VersionString);
  public CultureInfo Culture => CultureInfo.GetCultureInfo(CultureString);
  
  [NotNullIfNotNull(nameof(WebsiteUriString))]
  public Uri? WebsiteUri => WebsiteUriString is null ? null : new Uri(WebsiteUriString);
  
  [NotNullIfNotNull(nameof(Base64Image))]
  public Image? Image => Base64Image is null ? null 
    : Image.FromStream(new MemoryStream(Convert.FromBase64String(Base64Image)), true);
};