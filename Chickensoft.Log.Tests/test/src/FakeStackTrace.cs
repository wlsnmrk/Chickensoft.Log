namespace Chickensoft.Log.Tests;

using System.Diagnostics;

internal sealed class FakeStackTrace : StackTrace {
  private readonly string? _fileName;
  private readonly string? _className;
  private readonly string? _methodName;

  public FakeStackTrace(
    string? fileName, string? className, string? methodName
  ) {
    _fileName = fileName;
    _className = className;
    _methodName = methodName;
  }

  public override StackFrame GetFrame(int index)
    => new FakeStackFrame(_fileName, _className, _methodName);

  public override StackFrame[] GetFrames() => [
    new FakeStackFrame(_fileName, _className, _methodName),
  ];
}
