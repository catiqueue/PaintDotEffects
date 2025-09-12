namespace catiqueue.PaintDotNet.Plugins.PaintDotRnd;

internal sealed record Settings(int Seed, int Precision, int Zoom) {
  public static Settings Default => new(-1, 2, 1);
  public void Deconstruct(out int seed, out int precision, out int zoom) => (seed, precision, zoom) = (Seed, Precision, Zoom);
};