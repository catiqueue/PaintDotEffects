namespace catiqueue.PaintDotNet.Plugins.Common.Exceptions;

public class IncompleteDefinitionException(string nameOfBuilder, params string[] missingValues)
  : InvalidStateException(
    $"The {nameOfBuilder} is missing the following parameter (-s): {string.Join(", ", missingValues)}.") {
  public IncompleteDefinitionException(string nameOfBuilder, string missingValue) : this(nameOfBuilder, [missingValue]) { }
}