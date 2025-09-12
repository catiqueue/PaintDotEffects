using catiqueue.PaintDotNet.Plugins.Common.Data;

namespace PaintDotTests;

public class RangeTests {
  [Fact] public void TestAddition() {
    Assert.Equal(new Range<int>(0, 2), Range<int>.Normalized + Range<int>.Normalized);
    Assert.Equal(new Range<int>(-2, 2), new Range<int>(-1, 1) + new Range<int>(-1, 1));
  }
  
  [Fact] public void TestSubtraction() {
    Assert.Equal(new Range<int>(-1, 1), Range<int>.Normalized - Range<int>.Normalized);
    Assert.Equal(new Range<int>(-2, 2), new Range<int>(-1, 1) - new Range<int>(-1, 1));
  }
  
  [Fact] public void TestMultiplication() {
    Assert.Equal(new Range<int>(0, 1), Range<int>.Normalized * Range<int>.Normalized);
    Assert.Equal(new Range<int>(-2, 4), new Range<int>(-1, 2) * new Range<int>(-1, 2));
  }
  
  [Fact] public void TestDivision() {
    Assert.Throws<DivideByZeroException>(() => new Range<int>(1, 1) / new Range<int>(-1, 1));
    // Assert.Equal(new Range<int>(-1, 1), Range<int>.Normalized / Range<int>.Normalized);
    // Assert.Equal(new Range<int>(-2, 2), new Range<int>(-1, 1) / new Range<int>(-1, 1));
  }
}
