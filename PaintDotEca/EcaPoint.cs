namespace catiqueue.PaintDotNet.Plugins.PaintDotEca;

internal enum EcaPointDescriptor : sbyte { None = -2, Pregenerated = -1, Zero = 0, One, Two, Three, Four, Five, Six, Seven }

internal readonly record struct EcaPoint(EcaPointDescriptor Descriptor, bool IsActive) {
  public static EcaPoint Active(EcaPointDescriptor descriptor) => new(descriptor, true);
  public static EcaPoint Inactive(EcaPointDescriptor descriptor) => new(descriptor, false);

  public static EcaPoint FromRule(EcaParents parents, byte rule)
    => ((rule >> parents.AsByte) & 1) != 0
      ? Active(parents.ChildDescriptor)
      : Inactive(parents.ChildDescriptor);
};