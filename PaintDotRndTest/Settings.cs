using System;

namespace catiqueue.PaintDotNet.Plugins.PaintDotRndTest;

internal class Settings() : ICloneable {
  public int Seed { get; set; } = 0;
  public int Precision { get; set; } = 2;
  public int Zoom { get; set; } = 1;
  public object Clone() => new Settings { Seed = Seed, Precision = Precision, Zoom = Zoom };
};