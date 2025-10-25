namespace Chickensoft.Log;

using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// An <see cref="ILogWriter"/> that directs output of an <see cref="ILog"/>
/// to the console.
/// </summary>

// Excluded from coverage because Console output is untestable
[ExcludeFromCodeCoverage]
public sealed class ConsoleWriter : ILogWriter
{
  /// <inheritdoc/>
  public void WriteError(string message) => Console.Error.WriteLine(message);

  /// <inheritdoc/>
  public void WriteMessage(string message) => Console.WriteLine(message);

  /// <inheritdoc/>
  public void WriteWarning(string message) => Console.WriteLine(message);
}
