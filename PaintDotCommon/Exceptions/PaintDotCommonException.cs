using System;

namespace catiqueue.PaintDotNet.Plugins.Common.Exceptions;

public abstract class PaintDotCommonException : Exception {
  protected PaintDotCommonException() { }
  protected PaintDotCommonException(string message) : base(message) { }
  protected PaintDotCommonException(string message, Exception inner) : base(message, inner) { }
}