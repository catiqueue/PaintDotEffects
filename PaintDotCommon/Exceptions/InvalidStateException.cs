using System;

namespace catiqueue.PaintDotNet.Plugins.Common.Exceptions;

public class InvalidStateException : PaintDotCommonException {
  public InvalidStateException(string message) : base(message) { }
  public InvalidStateException(string message, Exception inner) : base(message, inner) { }
}