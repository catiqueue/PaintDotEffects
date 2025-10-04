namespace catiqueue.PaintDotNet.Plugins.Common.Exceptions;

public class MutuallyExclusiveException(string param1, string param2) 
  : InvalidStateException($"These parameters are mutually exclusive: {param1}, {param2}");