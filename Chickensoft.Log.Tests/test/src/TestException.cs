namespace Chickensoft.Log.Tests;

using System;

internal sealed class TestException : Exception {
  public TestException() { }
  public TestException(string msg) : base(msg) { }
  public TestException(string msg, Exception inner) : base(msg, inner) { }
}
