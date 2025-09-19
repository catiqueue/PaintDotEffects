namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal readonly record struct EcaParents(bool Left, bool Center, bool Right) {
  public EcaPointDescriptor ChildDescriptor { get; } = (Left, Center, Right) switch {
    { Left: true,  Center: true,  Right: true}  => EcaPointDescriptor.Seven,
    { Left: true,  Center: true,  Right: false} => EcaPointDescriptor.Six,
    { Left: true,  Center: false, Right: true}  => EcaPointDescriptor.Five,
    { Left: true,  Center: false, Right: false} => EcaPointDescriptor.Four,
    { Left: false, Center: true,  Right: true}  => EcaPointDescriptor.Three,
    { Left: false, Center: true,  Right: false} => EcaPointDescriptor.Two,
    { Left: false, Center: false, Right: true}  => EcaPointDescriptor.One,
    { Left: false, Center: false, Right: false} => EcaPointDescriptor.Zero
  };

  public byte AsByte => (byte) ChildDescriptor;
}