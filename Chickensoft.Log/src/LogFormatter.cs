namespace Chickensoft.Log;

/// <summary>
/// The default <see cref="ILogFormatter"/> for logs in Chickensoft.Log.
/// Provides a simple formatting structure of "LEVEL (NAME): MESSAGE", where:
/// <list type="bullet">
/// <item>LEVEL is the level of the log message (by default, Info, Warn, or
/// Error)</item>
/// <item>NAME is the name of the log from which the message originates</item>
/// <item>MESSAGE is the text of the log message</item>
/// </list>
/// </summary>
public class LogFormatter : ILogFormatter {
  /// <summary>
  /// The default level string for ordinary log messages. Defaults to "Info".
  /// </summary>
  /// <remarks>
  /// If changed, LogFormatters created in the future will use the new value,
  /// but existing LogFormatters will continue to use the values they were
  /// assigned on creation.
  /// </remarks>
  public static string DefaultMessagePrefix { get; set; } = "Info";

  /// <summary>
  /// The default level string for warning log messages. Defaults to "Warn".
  /// </summary>
  /// <remarks>
  /// If changed, LogFormatters created in the future will use the new value,
  /// but existing LogFormatters will continue to use the values they were
  /// assigned on creation.
  /// </remarks>
  public static string DefaultWarningPrefix { get; set; } = "Warn";

  /// <summary>
  /// The default level string for error log messages. Defaults to "Error".
  /// </summary>
  /// <remarks>
  /// If changed, LogFormatters created in the future will use the new value,
  /// but existing LogFormatters will continue to use the values they were
  /// assigned on creation.
  /// </remarks>
  public static string DefaultErrorPrefix { get; set; } = "Error";

  /// <summary>
  /// The level string for ordinary log messages. Defaults to the value
  /// of <see cref="DefaultMessagePrefix"/>.
  /// </summary>
  public string MessagePrefix { get; set; } = DefaultMessagePrefix;

  /// <summary>
  /// The level string for warning log messages. Defaults to the value
  /// of <see cref="DefaultWarningPrefix"/>.
  /// </summary>
  public string WarningPrefix { get; set; } = DefaultWarningPrefix;

  /// <summary>
  /// The level string for error log messages. Defaults to the value
  /// of <see cref="DefaultErrorPrefix"/>.
  /// </summary>
  public string ErrorPrefix { get; set; } = DefaultErrorPrefix;

  private static string Format(string level, string logName, string message) {
    return $"{level} ({logName}): {message}";
  }

  /// <inheritdoc/>
  public string FormatMessage(string logName, string message) {
    return Format(MessagePrefix, logName, message);
  }

  /// <inheritdoc/>
  public string FormatWarning(string logName, string message) {
    return Format(WarningPrefix, logName, message);
  }

  /// <inheritdoc/>
  public string FormatError(string logName, string message) {
    return Format(ErrorPrefix, logName, message);
  }
}
