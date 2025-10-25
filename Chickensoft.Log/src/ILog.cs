namespace Chickensoft.Log;

using System;
using System.Diagnostics;

/// <summary>
/// Log interface for outputting messages produced during runtime.
/// A debug implementation might print the messages to the console, while a
/// production implementation might write the messages to a file.
/// </summary>
public interface ILog
{
  /// <summary>
  /// The name associated with this log. Will be used in output so that all
  /// messages from a single log can be identified.
  /// </summary>
  string Name { get; }

  /// <summary>
  /// The formatter that will be used to format messages before writing them
  /// to the console.
  /// </summary>
  ILogFormatter Formatter { get; set; }

  /// <summary>
  /// Adds a writer to this log, if it is not already present.
  /// </summary>
  /// <param name="writer">The writer to add.</param>
  void AddWriter(ILogWriter writer);

  /// <summary>
  /// Removes a writer from this log, if it is present.
  /// </summary>
  /// <param name="writer">The writer to remove.</param>
  void RemoveWriter(ILogWriter writer);

  /// <summary>
  /// Prints the specified message to the log.
  /// </summary>
  /// <param name="message">Message to output.</param>
  void Print(string message);

  /// <summary>
  /// Displays a stack trace in a convenient format.
  /// </summary>
  /// <param name="stackTrace">Stack trace to output.</param>
  void Print(StackTrace stackTrace);

  /// <summary>
  /// Displays a stack trace in a convenient format, with a message for context.
  /// </summary>
  /// <param name="stackTrace">Stack trace to output.</param>
  /// <param name="message">Message to output.</param>
  void Print(StackTrace stackTrace, string message);

  /// <summary>
  /// Prints an exception as an error. This method is an alias for
  /// <see cref="Err(Exception)"/>.
  /// </summary>
  /// <param name="e">Exception to print.</param>
  /// <seealso cref="Err(Exception)"/>
  void Print(Exception e);

  /// <summary>
  /// Prints an exception as an error, with a message for context. This method
  /// is an alias for <see cref="Err(Exception, string)"/>.
  /// </summary>
  /// <param name="e">Exception to print.</param>
  /// <param name="message">Message to output.</param>
  /// <seealso cref="Err(Exception, string)"/>
  void Print(Exception e, string message);

  /// <summary>
  /// Adds a warning message to the log.
  /// </summary>
  /// <param name="message">Message to output.</param>
  void Warn(string message);

  /// <summary>
  /// Adds an error message to the log.
  /// </summary>
  /// <param name="message">Message to output.</param>
  void Err(string message);

  /// <summary>
  /// Prints an exception as an error.
  /// </summary>
  /// <param name="e">Exception to print.</param>
  void Err(Exception e);

  /// <summary>
  /// Prints an exception as an error, with a message for context.
  /// </summary>
  /// <param name="e">Exception to print.</param>
  /// <param name="message">Message to output.</param>
  void Err(Exception e, string message);
}
