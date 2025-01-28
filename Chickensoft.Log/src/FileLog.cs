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

    /// <summary>
    /// The path of the filename this Writer is writing to.
    /// </summary>
    public string FileName { get; }

    private bool _isCleared;

    private Writer(string fileName) {
      FileName = fileName;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style",
      "IDE0063:Use simple 'using' statement",
      Justification = "Prefer block, to explicitly delineate scope")]
    private void WriteLine(string message) {
      // Clearing the file here, instead of at construction, prevents us from
      // overwriting files until something's actually logged, and also supports
      // more testing
      if (!_isCleared) {
        using (var sw = new StreamWriter(FileName)) { }
        _isCleared = true;
      }
      using (var sw = File.AppendText(FileName)) {
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

  private readonly IWriter _writer;

  /// <inheritdoc/>
  public string Name { get; }

  /// <summary>
  /// The formatter that will be used to format messages before writing them
  /// to the file. Defaults to an instance of <see cref="LogFormatter"/>.
  /// </summary>
  public ILogFormatter Formatter { get; set; } = new LogFormatter();

  /// <summary>
  /// Create a <see cref="FileLog"/> using the given name and default
  /// file name (<see cref="Writer.DefaultFileName"/>).
  /// </summary>
  /// <param name="name">
  /// The name associated with this log. Will be included in messages directed
  /// through this log (see <see cref="Name"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  public FileLog(string name) {
    _writer = Writer.Instance();
    Name = name;
  }

  /// <summary>
  /// Create a <see cref="FileLog"/> using the given name,
  /// outputting to the given file name.
  /// </summary>
  /// <param name="name">
  /// The name associated with this log. Will be included in messages directed
  /// through this log (see <see cref="Name"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  /// <param name="fileName">
  /// The path for the file where logs should be written.
  /// </param>
  public FileLog(string name, string fileName) {
    _writer = Writer.Instance(fileName);
    Name = name;
  }

  /// <summary>
  /// Create a <see cref="FileLog"/> using the given name and
  /// <see cref="IWriter"/> to output. Useful for testing.
  /// </summary>
  /// <param name="name">
  /// The name associated with this log. Will be included in messages directed
  /// through this log (see <see cref="Name"/>).
  /// A common value is <c>nameof(EncapsulatingClass)</c>.
  /// </param>
  /// <param name="writer">
  /// The writer to use for outputting log messages.
  /// </param>
  public FileLog(string name, IWriter writer) {
    _writer = writer;
    Name = name;
  }

  /// <inheritdoc/>
  public void Err(string message) {
    _writer.WriteError(Formatter.FormatError(Name, message));
  }

  /// <inheritdoc/>
  public void Print(string message) {
    _writer.WriteMessage(Formatter.FormatMessage(Name, message));
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
      Print(
        $"{className}.{methodName} in " +
        $"{fileName}({lineNumber},{colNumber})"
      );
    }
  }

  /// <inheritdoc/>
  public void Print(Exception e) {
    Err("Exception:");
    Err(e.ToString());
  }

  /// <inheritdoc/>
  public void Warn(string message) {
    _writer.WriteWarning(Formatter.FormatWarning(Name, message));
  }
}
