namespace Chickensoft.Log;

/// <summary>
/// Writer interface providing file-writing functionality to
/// <see cref="FileLog"/>.
/// </summary>
public interface ILogWriter {
  /// <summary>
  /// Appends the given message to this <see cref="ILogWriter"/>'s output, on a
  /// new line.
  /// </summary>
  /// <param name="message">
  /// The string to write to this <see cref="ILogWriter"/>'s output.
  /// </param>
  void WriteMessage(string message);

  /// <summary>
  /// Appends the given warning to this <see cref="ILogWriter"/>'s output, on a
  /// new line.
  /// </summary>
  /// <param name="message">
  /// The warning to write to this <see cref="ILogWriter"/>'s output.
  /// </param>
  void WriteWarning(string message);

  /// <summary>
  /// Appends the given error to this <see cref="ILogWriter"/>'s output, on a
  /// new line.
  /// </summary>
  /// <param name="message">
  /// The error to write to this <see cref="ILogWriter"/>'s output.
  /// </param>
  void WriteError(string message);
}
