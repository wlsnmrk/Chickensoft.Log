namespace Chickensoft.Log;

using System;
using System.Diagnostics;

/// <summary>
/// An <see cref="ILog"/> that writes to standard output and error.
/// </summary>
public class ConsoleLog : ILog {
  /// <summary>
  /// An <see cref="ILogWriter"/> implementing output for <see cref="ConsoleLog"/>.
  /// </summary>
  public interface IWriter : ILogWriter;

  /// <summary>
  /// An <see cref="IWriter"/> that directs output of a <see cref="ConsoleLog"/>
  /// to the console.
  /// </summary>
  public class Writer : IWriter {
    /// <inheritdoc/>
    public void WriteError(string message) {
      Console.Error.WriteLine(message);
    }

    /// <inheritdoc/>
    public void WriteMessage(string message) {
      Console.WriteLine(message);
    }

    /// <inheritdoc/>
    public void WriteWarning(string message) {
      Console.WriteLine(message);
    }
  }

  private readonly IWriter _writer;

  /// <inheritdoc/>
  public string Name { get; }

  /// <summary>
  /// The formatter that will be used to format messages before writing them
  /// to the console. Defaults to an instance of <see cref="LogFormatter"/>.
  /// </summary>
  public ILogFormatter Formatter { get; set; } = new LogFormatter();

  /// <summary>
  /// Create a logger using the given name and standard out/err.
  /// </summary>
  /// <param name="name">
  /// The name associated with this log. Will be included in messages directed
  /// through this log (see <see cref="Name"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  public ConsoleLog(string name) {
    Name = name;
    _writer = new Writer();
  }

  /// <summary>
  /// Create a logger using the given name and the provided
  /// <see cref="IWriter"/> for output. Useful for testing.
  /// </summary>
  /// <param name="name">
  /// The name associated with this log. Will be included in messages directed
  /// through this log (see <see cref="Name"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  /// <param name="writer">
  /// The writer to use for outputting log messages.
  /// </param>
  public ConsoleLog(string name, IWriter writer) {
    Name = name;
    _writer = writer;
  }

  /// <inheritdoc/>
  public void Err(string message) {
    _writer.WriteError(Formatter.FormatError(Name, message));
  }

  /// <inheritdoc/>
  public void Print(string message) {
    _writer.WriteMessage(Formatter.FormatMessage(Name, message));
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
    _writer.WriteWarning(Formatter.FormatWarning(Name, message));
  }
}
