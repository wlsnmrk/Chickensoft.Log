namespace Chickensoft.Log;

using System.Diagnostics;

/// <summary>
/// An <see cref="ILogWriter"/> that directs output of an <see cref="ILog"/>
/// to <see cref="Trace"/>.
/// </summary>
public sealed class TraceWriter : ILogWriter {
  private static readonly TraceWriter _instance = new();

  /// <summary>
  /// Returns a reference to the singleton instance of TraceWriter.
  /// </summary>
  public static TraceWriter Instance() {
    return _instance;
  }

  private readonly object _writingLock = new();

  /// <inheritdoc/>
  public void WriteMessage(string message) {
    lock (_writingLock) {
      Trace.WriteLine(message);
    }
  }

  /// <inheritdoc/>
  public void WriteError(string message) {
    lock (_writingLock) {
      Trace.TraceError(message);
    }
  }

  /// <inheritdoc/>
  public void WriteWarning(string message) {
    lock (_writingLock) {
      Trace.TraceWarning(message);
    }
  }
}
