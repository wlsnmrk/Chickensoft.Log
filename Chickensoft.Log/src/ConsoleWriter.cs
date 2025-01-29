namespace Chickensoft.Log;

using System;

/// <summary>
/// An <see cref="ILogWriter"/> that directs output of an <see cref="ILog"/>
/// to the console.
/// </summary>
public sealed class ConsoleWriter : ILogWriter {
  private static readonly ConsoleWriter _instance = new();

  /// <summary>
  /// Returns a reference to the singleton instance of ConsoleWriter.
  /// </summary>
  public static ConsoleWriter Instance() {
    return _instance;
  }

  private readonly object _writingLock = new();

  private ConsoleWriter() { }

  /// <inheritdoc/>
  public void WriteError(string message) {
    lock (_writingLock) {
      Console.Error.WriteLine(message);
    }
  }

  /// <inheritdoc/>
  public void WriteMessage(string message) {
    lock (_writingLock) {
      Console.WriteLine(message);
    }
  }

  /// <inheritdoc/>
  public void WriteWarning(string message) {
    lock (_writingLock) {
      Console.WriteLine(message);
    }
  }
}
