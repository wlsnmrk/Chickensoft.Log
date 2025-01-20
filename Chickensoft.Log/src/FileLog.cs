namespace Chickensoft.Log;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

/// <summary>
/// An <see cref="ILog"/> that writes to a file.
/// </summary>
public sealed class FileLog : ILog {
  /// <summary>
  /// An <see cref="ILogWriter"/> implementing output for <see cref="FileLog"/>.
  /// </summary>
  public interface IWriter : ILogWriter;

  /// <summary>
  /// An <see cref="IWriter"/> that directs output of a <see cref="FileLog"/>
  /// to a particular file.
  /// </summary>
  public sealed class Writer : IWriter {
    // Implemented as a pseudo-singleton to enforce one truncation per file per
    // execution
    private static readonly Dictionary<string, Writer> _instances = [];

    /// <summary>
    /// The default file name that will be used when creating a
    /// <see cref="Writer"/> if no filename is specified.
    /// </summary>
    /// <remarks>
    /// This default may be changed. If it is changed after a default
    /// <see cref="Writer"/> has already been created, any future calls to
    /// <see cref="Instance()"/> will return a different <see cref="Writer"/>
    /// outputting to the new default, but previously-created instances will not
    /// be changed and will continue outputting to the original default file.
    /// </remarks>
    public static string DefaultFileName { get; set; } = "output.log";

    /// <summary>
    /// Obtains a <see cref="Writer"/> that directs output to the given filename.
    /// </summary>
    /// <param name="fileName">
    /// The filename to which output should be directed when using the returned
    /// <see cref="Writer"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Writer"/> outputting to a file at
    /// <paramref name="fileName"/>.
    /// </returns>
    /// <remarks>
    /// If a <see cref="Writer"/> outputting to <paramref name="fileName"/>
    /// already exists, a reference to the same <see cref="Writer"/> will be
    /// returned. If not, a new <see cref="Writer"/> will be created. When a new
    /// <see cref="Writer"/> is created, if the file at
    /// <paramref name="fileName"/> already exists, it will erased; if not, it
    /// will be created.
    /// </remarks>
    public static Writer Instance(string fileName) {
      if (_instances.TryGetValue(fileName, out var writer)) {
        return writer;
      }
      // Clear the file
      using (var sw = new StreamWriter(fileName)) { }
      writer = new Writer(fileName);
      _instances[fileName] = writer;
      return writer;
    }

    /// <summary>
    /// Obtains a <see cref="Writer"/> that directs output to the current
    /// <see cref="DefaultFileName"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Writer"/> outputting to a file at
    /// <see cref="DefaultFileName"/>.
    /// </returns>
    /// <seealso cref="Instance(string)"/>
    /// <seealso cref="DefaultFileName"/>
    public static Writer Instance() {
      return Instance(DefaultFileName);
    }

    private readonly string _fileName;

    private Writer(string fileName) {
      _fileName = fileName;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style",
      "IDE0063:Use simple 'using' statement",
      Justification = "Prefer block, to explicitly delineate scope")]
    private void WriteLine(string message) {
      using (var sw = File.AppendText(_fileName)) {
        sw.WriteLine(message);
      }
    }

    /// <inheritdoc/>
    public void WriteMessage(string message) {
      WriteLine(message);
    }

    /// <inheritdoc/>
    public void WriteWarning(string message) {
      WriteLine(message);
    }

    /// <inheritdoc/>
    public void WriteError(string message) {
      WriteLine(message);
    }
  }

  private const string WARNING_PREFIX = "WARNING";
  private const string ERROR_PREFIX = "ERROR";

  private readonly IWriter _writer;

  /// <summary>
  /// The prefix string which will be prepended to all messages before output.
  /// </summary>
  public string Prefix { get; }

  /// <summary>
  /// Create a <see cref="FileLog"/> using the given prefix string and default
  /// file name (<see cref="Writer.DefaultFileName"/>).
  /// </summary>
  /// <param name="prefix">
  /// The prefix string to prepend to messages directed through this logger (see
  /// <see cref="Prefix"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  public FileLog(string prefix) {
    _writer = Writer.Instance();
    Prefix = prefix;
  }

  /// <summary>
  /// Create a <see cref="FileLog"/> using the given prefix string and
  /// outputting to the given file name.
  /// </summary>
  /// <param name="prefix">
  /// The prefix string to prepend to messages directed through this logger (see
  /// <see cref="Prefix"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  /// <param name="fileName">
  /// The path for the file where logs should be written.
  /// </param>
  public FileLog(string prefix, string fileName) {
    _writer = Writer.Instance(fileName);
    Prefix = prefix;
  }

  /// <summary>
  /// Create a <see cref="FileLog"/> using the given prefix string and
  /// <see cref="IWriter"/> to output to a particular file. Useful for testing.
  /// </summary>
  /// <param name="prefix">
  /// The prefix string to prepend to messages directed through this logger (see
  /// <see cref="Prefix"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  /// <param name="writer">
  /// The writer to use for outputting log messages.
  /// </param>
  public FileLog(string prefix, IWriter writer) {
    _writer = writer;
    Prefix = prefix;
  }

  /// <inheritdoc/>
  public void Err(string message) {
    _writer.WriteError(ERROR_PREFIX + " in " + Prefix + ": " + message);
  }

  /// <inheritdoc/>
  public void Print(string message) {
    _writer.WriteMessage(Prefix + ": " + message);
  }

  /// <inheritdoc/>
  public void Print(StackTrace stackTrace) {
    foreach (var frame in stackTrace.GetFrames()) {
      var fileName = frame.GetFileName() ?? "**";
      var lineNumber = frame.GetFileLineNumber();
      var colNumber = frame.GetFileColumnNumber();
      var method = frame.GetMethod();
      var className = method?.DeclaringType?.Name ?? "UnknownClass";
      var methodName = method?.Name ?? "UnknownMethod";
      Err(
        $"{className}.{methodName} in " +
        $"{fileName}({lineNumber},{colNumber})"
      );
    }
  }

  /// <inheritdoc/>
  public void Print(Exception e) {
    Err("An error occurred.");
    Err(e.ToString());
  }

  /// <inheritdoc/>
  public void Warn(string message) {
    _writer.WriteWarning(WARNING_PREFIX + " in " + Prefix + ": " + message);
  }
}
