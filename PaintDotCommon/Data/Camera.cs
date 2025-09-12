namespace catiqueue.PaintDotNet.Plugins.Common.Data;

public readonly record struct Camera(Vector<int> Offset, int Zoom) {
  public static Camera Default => new(Vector<int>.Zero, Zoom: 1);
  public Vector<int> ApplyTo(Vector<int> pos) => pos / Zoom + Offset;
}