namespace Chickensoft.Log;

/// <summary>
/// Optional interface providing log-message formatting to <see cref="ILog"/>.
/// </summary>
public interface ILogFormatter {
  /// <summary>
  /// Formats a standard informational log message.
  /// </summary>
  /// <param name="logName">
  /// The name of the log from which the message
  /// originates.
  /// </param>
  /// <param name="message">The log message to format.</param>
  /// <returns>
  /// A formatted version of the message including the log name and
  /// the log level.
  /// </returns>
  public string FormatMessage(string logName, string message);

  /// <summary>
  /// Formats a warning message.
  /// </summary>
  /// <param name="logName">
  /// The name of the log from which the warning message
  /// originates.
  /// </param>
  /// <param name="message">The warning message to format.</param>
  /// <returns>
  /// A formatted version of the message including the log name and
  /// the log level.
  /// </returns>
  public string FormatWarning(string logName, string message);

  /// <summary>
  /// Formats a warning message.
  /// </summary>
  /// <param name="logName">
  /// The name of the log from which this error message
  /// originates.
  /// </param>
  /// <param name="message">The error message to format.</param>
  /// <returns>
  /// A formatted version of the message including the log name and
  /// the log level.
  /// </returns>
  public string FormatError(string logName, string message);
}
