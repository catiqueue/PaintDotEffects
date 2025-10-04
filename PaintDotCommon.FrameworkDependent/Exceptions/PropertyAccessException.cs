namespace catiqueue.PaintDotNet.Plugins.Common.Exceptions;

public class PropertyAccessException(string name) 
  : InvalidStateException($"The property with name {name} was not found in the PropertyCollection.");