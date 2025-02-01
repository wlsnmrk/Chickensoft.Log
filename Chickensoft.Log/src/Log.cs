namespace Chickensoft.Log;

using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// The standard implementation of <see cref="ILog"/>.
/// </summary>
public sealed class Log : ILog {
  private readonly object _writersLock = new();

  /// <inheritdoc/>
  public string Name { get; }

  internal readonly List<ILogWriter> _writers = [];

  /// <summary>
  /// The formatter that will be used to format messages before writing them
  /// to the console. Defaults to an instance of <see cref="LogFormatter"/>.
  /// </summary>
  public ILogFormatter Formatter { get; set; } = new LogFormatter();

  /// <summary>
  /// Initialize a log that will use the provided writers.
  /// </summary>
  /// <param name="name">
  /// The name associated with this log. Will be included in messages directed
  /// through this log (see <see cref="Name"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  /// <param name="writers">Writers this log will use to write messages.</param>
  public Log(string name, params ILogWriter[] writers) {
    Name = name;
    _writers = [.. writers];
  }

  /// <summary>
  /// Creates a log that will output to trace by default.
  /// </summary>
  /// <param name="name">
  /// The name associated with this log. Will be included in messages directed
  /// through this log (see <see cref="Name"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  public Log(string name) {
    Name = name;
    _writers = [new TraceWriter()];
  }

  /// <inheritdoc/>
  public void AddWriter(ILogWriter writer) {
    lock (_writersLock) {
      if (!_writers.Contains(writer)) {
        _writers.Add(writer);
      }
    }
  }

  /// <inheritdoc/>
  public void RemoveWriter(ILogWriter writer) {
    lock (_writersLock) {
      _writers.Remove(writer);
    }
  }

  /// <inheritdoc/>
  public void Print(string message) {
    var formatted = Formatter.FormatMessage(Name, message);
    lock (_writersLock) {
      foreach (var writer in _writers) {
        writer.WriteMessage(formatted);
      }
    }
  }

  /// <inheritdoc/>
  public void Print(StackTrace stackTrace) {
    var formatted = Formatter.FormatMessage(Name, stackTrace);
    lock (_writersLock) {
      foreach (var writer in _writers) {
        writer.WriteMessage(formatted);
      }
    }
  }

  /// <inheritdoc/>
  public void Print(StackTrace stackTrace, string message) {
    var formattedStackTrace = Formatter.FormatMessage(Name, stackTrace);
    var formattedMessage = Formatter.FormatMessage(Name, message);
    lock (_writersLock) {
      foreach (var writer in _writers) {
        writer.WriteMessage(formattedMessage);
        writer.WriteMessage(formattedStackTrace);
      }
    }
  }

  /// <inheritdoc/>
  public void Print(Exception e) {
    lock (_writersLock) {
      Err("Exception:");
      Err(e.ToString());
    }
  }

  /// <inheritdoc/>
  public void Print(Exception e, string message) {
    lock (_writersLock) {
      Err(message);
      Print(e);
    }
  }

  /// <inheritdoc/>
  public void Warn(string message) {
    var formatted = Formatter.FormatWarning(Name, message);
    lock (_writersLock) {
      foreach (var writer in _writers) {
        writer.WriteWarning(formatted);
      }
    }
  }

  /// <inheritdoc/>
  public void Err(string message) {
    var formatted = Formatter.FormatError(Name, message);
    lock (_writersLock) {
      foreach (var writer in _writers) {
        writer.WriteError(formatted);
      }
    }
  }
}
