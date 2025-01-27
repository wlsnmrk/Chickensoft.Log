namespace Chickensoft.Log;

using System;
using System.Diagnostics;
using System.Text;

/// <summary>
/// An <see cref="ILog"/> that accumulates messages written to it so users can
/// test their logging code without mocking ILog.
/// </summary>
public class TestLog : ILog {
  /// <summary>
  /// The name associated with this log. Always has the value "Test".
  /// </summary>
  public string Name { get; } = "Test";

  /// <summary>
  /// The formatter that will be used to format messages before writing them
  /// to the console. Defaults to an instance of <see cref="LogFormatter"/>.
  /// </summary>
  public ILogFormatter Formatter { get; set; } = new LogFormatter();

  /// <summary>
  /// Contains all logged text.
  /// </summary>
  public StringBuilder LoggedTextBuilder { get; set; } = new();

  /// <inheritdoc/>
  public void Err(string message) {
    LoggedTextBuilder.AppendLine(Formatter.FormatError(Name, message));
  }

  /// <inheritdoc/>
  public void Print(string message) {
    LoggedTextBuilder.AppendLine(Formatter.FormatMessage(Name, message));
  }

  /// <inheritdoc/>
  public void Print(StackTrace stackTrace) {
    foreach (var frame in stackTrace.GetFrames()) {
      var fileName = frame.GetFileName() ?? "**";
      var lineNumber = frame.GetFileLineNumber();
      var colNumber = frame.GetFileColumnNumber();
      var method = frame.GetMethod();
      var className = method?.DeclaringType?.Name ?? "UnknownClass";
      var methodName = method?.Name ?? "UnknownMethod";
      Print(
        $"{className}.{methodName} in " +
        $"{fileName}({lineNumber},{colNumber})"
      );
    }
  }

  /// <inheritdoc/>
  public void Print(Exception e) {
    Err("Exception:");
    Err(e.ToString());
  }

  /// <inheritdoc/>
  public void Warn(string message) {
    LoggedTextBuilder.AppendLine(Formatter.FormatWarning(Name, message));
  }
}
