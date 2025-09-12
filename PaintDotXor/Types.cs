using catiqueue.PaintDotNet.Plugins.Common.Data;
using PaintDotNet.Imaging;

namespace catiqueue.PaintDotNet.Plugins.PaintDotXor.Types;

internal delegate bool Filter(int magic);
internal delegate ManagedColor Painter(int magic);
internal delegate int Operation(Vector<int> pos);

internal enum OperationChoice { XOR, AND, OR }
internal enum FilterChoice { IsPrime, IsDivisible }