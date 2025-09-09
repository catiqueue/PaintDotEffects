namespace catiqueue.PaintDotNet.Plugins.Common.Data;

public readonly record struct Camera(Vector2I Offset, int Zoom) {
  public static Camera Default => new(Vector2I.Zero, Zoom: 1);
  public Vector2I ApplyTo(Vector2I pos) => pos / Zoom + Offset;
}