namespace Chickensoft.Log.Tests;

using System.Diagnostics;
using System.Reflection;

internal sealed class FakeStackFrame : StackFrame
{
  private readonly string? _fileName;
  private readonly string? _className;
  private readonly string? _methodName;

  public FakeStackFrame(
    string? fileName, string? className, string? methodName
  )
  {
    _fileName = fileName;
    _methodName = methodName;
    _className = className;
  }

  public override string? GetFileName() => _fileName;
  public override int GetFileLineNumber() => 1;
  public override int GetFileColumnNumber() => 2;
  public override MethodBase? GetMethod()
    => (_methodName != null)
      ? new FakeMethodBase(_fileName, _className, _methodName)
      : null;
}
