namespace Chickensoft.Log;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// An <see cref="ILogWriter"/> that directs output of an <see cref="ILog"/>
/// to <see cref="Trace"/>.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Trace output is untestable")]
public sealed class TraceWriter : ILogWriter {
  /// <inheritdoc/>
  public void WriteMessage(string message) {
    Trace.WriteLine(message);
  }

  /// <inheritdoc/>
  public void WriteError(string message) {
    Trace.TraceError(message);
  }

  /// <inheritdoc/>
  public void WriteWarning(string message) {
    Trace.TraceWarning(message);
  }
}
