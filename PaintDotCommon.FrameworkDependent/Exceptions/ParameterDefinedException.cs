namespace catiqueue.PaintDotNet.Plugins.Common.Exceptions;

public class ParameterDefinedException(string nameOfBuilder, string parameterName)
  : PaintDotCommonException($"The parameter {parameterName} is already defined in {nameOfBuilder}.");