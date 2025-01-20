namespace Chickensoft.Log;
using System;
using System.Diagnostics;

/// <summary>
/// A logger that uses <see cref="Trace"/> to output log messages.
/// Useful for seeing output in Visual Studio's Output window while debugging.
/// </summary>
/// <remarks>
/// To enable <see cref="Trace"/> output in the VS Output window,
/// add a <see cref="DefaultTraceListener"/> to the list of Trace listeners
/// before any logging (i.e., near your entry point):
/// <code>
/// Trace.Listeners.Add(new DefaultTraceListener());
/// </code>
/// </remarks>
public sealed class TraceLog : ILog {
  /// <summary>
  /// An <see cref="ILogWriter"/> implementing output for <see cref="TraceLog"/>.
  /// </summary>
  public interface IWriter : ILogWriter;

  /// <summary>
  /// An <see cref="IWriter"/> that directs output of a <see cref="TraceLog"/>
  /// to <see cref="Trace"/>.
  /// </summary>
  public sealed class Writer : IWriter {
    /// <inheritdoc/>
    public void WriteMessage(string message) {
      Trace.WriteLine(message);
    }

    /// <inheritdoc/>
    public void WriteError(string message) {
      Trace.TraceError(message);
    }

    /// <inheritdoc/>
    public void WriteWarning(string message) {
      Trace.TraceWarning(message);
    }
  }

  private readonly IWriter _writer;

  /// <summary>
  /// The prefix string which will be prepended to all messages before output.
  /// </summary>
  public string Prefix { get; }

  /// <summary>
  /// Create a trace logger with the given prefix string.
  /// </summary>
  /// <param name="prefix">
  /// The prefix string to prepend to messages directed through this logger (see
  /// <see cref="Prefix"/>). A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  public TraceLog(string prefix) {
    Prefix = prefix;
    _writer = new Writer();
  }

  /// <summary>
  /// Create a trace logger with the given prefix string and writer. Useful
  /// for testing.
  /// </summary>
  /// <param name="prefix">
  /// The prefix string to prepend to messages directed through this logger (see
  /// <see cref="Prefix"/>). A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  /// <param name="writer">
  /// The writer to use for outputting log messages.
  /// </param>
  public TraceLog(string prefix, IWriter writer) {
    Prefix = prefix;
    _writer = writer;
  }

  /// <inheritdoc/>
  public void Err(string message) {
    _writer.WriteError($"{Prefix}: {message}");
  }

  /// <inheritdoc/>
  public void Print(string message) {
    _writer.WriteMessage($"{Prefix}: {message}");
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
      Err(
        $"{className}.{methodName} in " +
        $"{fileName}({lineNumber},{colNumber})"
      );
    }
  }

  /// <inheritdoc/>
  public void Print(Exception e) {
    Err("An error occurred.");
    Err(e.ToString());
  }

  /// <inheritdoc/>
  public void Warn(string message) {
    _writer.WriteWarning($"WARNING in {Prefix}: {message}");
  }
}
