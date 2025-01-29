namespace Chickensoft.Log;

using System.Collections.Generic;

/// <summary>
/// An <see cref="ILogWriter"/> that stores logged messages and does not write
/// them to any destination. Useful for testing code that uses
/// <see cref="ILog"/>.
/// </summary>
public sealed class TestWriter : ILogWriter {
  /// <summary>
  /// Contains all regular logged messages as separate elements.
  /// </summary>
  public IList<string> LoggedMessages { get; set; } = [];
  /// <summary>
  /// Contains all logged warning messages as separate elements.
  /// </summary>
  public IList<string> LoggedWarnings { get; set; } = [];
  /// <summary>
  /// Contains all logged error messages as separate elements.
  /// </summary>
  public IList<string> LoggedErrors { get; set; } = [];

  /// <summary>
  /// Clears all stored messages, of every level.
  /// </summary>
  public void Reset() {
    LoggedMessages.Clear();
    LoggedWarnings.Clear();
    LoggedErrors.Clear();
  }

  /// <inheritdoc/>
  public void WriteError(string message) {
    LoggedErrors.Add(message);
  }

  /// <inheritdoc/>
  public void WriteMessage(string message) {
    LoggedMessages.Add(message);
  }

  /// <inheritdoc/>
  public void WriteWarning(string message) {
    LoggedWarnings.Add(message);
  }
}
