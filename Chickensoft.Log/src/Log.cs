namespace Chickensoft.Log;

using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// The standard implementation of <see cref="ILog"/>.
/// </summary>
public sealed class Log : ILog {
  /// <inheritdoc/>
  public string Name { get; }

  /// <inheritdoc/>
  public IList<ILogWriter> Writers { get; } = [];

  /// <summary>
  /// The formatter that will be used to format messages before writing them
  /// to the console. Defaults to an instance of <see cref="LogFormatter"/>.
  /// </summary>
  public ILogFormatter Formatter { get; set; } = new LogFormatter();

  /// <summary>
  /// Initialize an empty Log (i.e., with no writers).
  /// </summary>
  /// <param name="name">
  /// The name associated with this log. Will be included in messages directed
  /// through this log (see <see cref="Name"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  public Log(string name) {
    Name = name;
  }

  /// <summary>
  /// Initialize a Log that will use the provided writers.
  /// </summary>
  /// <param name="name">
  /// The name associated with this log. Will be included in messages directed
  /// through this log (see <see cref="Name"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  /// <param name="writers">Writers this log will use to write messages.</param>
  public Log(string name, IList<ILogWriter> writers) {
    Name = name;
    Writers = [.. writers];
  }

  /// <inheritdoc/>
  public void Print(string message) {
    var formatted = Formatter.FormatMessage(Name, message);
    foreach (var writer in Writers) {
      writer.WriteMessage(formatted);
    }
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
    var formatted = Formatter.FormatWarning(Name, message);
    foreach (var writer in Writers) {
      writer.WriteWarning(formatted);
    }
  }

  /// <inheritdoc/>
  public void Err(string message) {
    var formatted = Formatter.FormatError(Name, message);
    foreach (var writer in Writers) {
      writer.WriteError(formatted);
    }
  }
}
