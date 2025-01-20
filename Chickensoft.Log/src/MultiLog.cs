namespace Chickensoft.Log;

using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// An <see cref="ILog"/> that writes to multiple other ILogs. Provides a
/// convenient way to log to multiple outputs (e.g., to
/// <see cref="Trace"/> and to a file).
/// </summary>
public sealed class MultiLog : ILog {
  /// <summary>
  /// The logs to which this log will write all messages.
  /// </summary>
  public IList<ILog> Logs { get; } = [];

  /// <summary>
  /// Initialize an empty MultiLog (i.e., with no logs to write to).
  /// </summary>
  /// <seealso cref="Logs"/>
  public MultiLog() {
  }

  /// <summary>
  /// Initialize a MultiLog that will write to the provided logs.
  /// </summary>
  /// <param name="logs">Logs to which this MultiLog will write.</param>
  public MultiLog(IList<ILog> logs) {
    Logs = new List<ILog>(logs);
  }

  /// <inheritdoc/>
  public void Print(string message) {
    foreach (var log in Logs) {
      log.Print(message);
    }
  }

  /// <inheritdoc/>
  public void Print(StackTrace stackTrace) {
    foreach (var log in Logs) {
      log.Print(stackTrace);
    }
  }

  /// <inheritdoc/>
  public void Print(Exception e) {
    foreach (var log in Logs) {
      log.Print(e);
    }
  }

  /// <inheritdoc/>
  public void Warn(string message) {
    foreach (var log in Logs) {
      log.Warn(message);
    }
  }

  /// <inheritdoc/>
  public void Err(string message) {
    foreach (var log in Logs) {
      log.Err(message);
    }
  }
}
