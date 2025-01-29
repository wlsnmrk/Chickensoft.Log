namespace Chickensoft.Log;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

/// <summary>
/// An <see cref="ILogWriter"/> that directs output of an <see cref="ILog"/>
/// to a file.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "File output is untestable")]
public sealed class FileWriter : ILogWriter {
  // protect static members from simultaneous thread access
  private static readonly object _singletonLock = new();
  // Implemented as a pseudo-singleton to enforce one truncation per file per
  // execution
  private static readonly Dictionary<string, FileWriter> _instances = [];

  private static string _defaultFileName = "output.log";
  /// <summary>
  /// The default file name that will be used when creating a
  /// <see cref="FileWriter"/> if no filename is specified.
  /// </summary>
  /// <remarks>
  /// This default may be changed. If it is changed after a default
  /// <see cref="FileWriter"/> has already been created, any future calls to
  /// <see cref="Instance()"/> will return a different <see cref="FileWriter"/>
  /// outputting to the new default, but previously-created instances will not
  /// be changed and will continue outputting to the original default file.
  /// </remarks>
  public static string DefaultFileName {
    get {
      lock (_singletonLock) {
        return _defaultFileName;
      }
    }
    set {
      lock (_singletonLock) {
        _defaultFileName = value;
      }
    }
  }

  /// <summary>
  /// Obtains a FileWriter that directs output to the given filename.
  /// </summary>
  /// <param name="fileName">
  /// The filename to which output should be directed when using the returned
  /// writer.
  /// </param>
  /// <returns>
  /// A writer outputting to a file at <paramref name="fileName"/>.
  /// </returns>
  /// <remarks>
  /// If a <see cref="FileWriter"/> outputting to <paramref name="fileName"/>
  /// already exists, a reference to the same writer will be
  /// returned. If not, a new writer will be created. When a
  /// new <see cref="FileWriter"/> is created, if the file at
  /// <paramref name="fileName"/> already exists, it will erased; if not, it
  /// will be created.
  /// </remarks>
  public static FileWriter Instance(string fileName) {
    lock (_singletonLock) {
      if (_instances.TryGetValue(fileName, out var writer)) {
        return writer;
      }
      writer = new FileWriter(fileName);
      _instances[fileName] = writer;
      return writer;
    }
  }

  /// <summary>
  /// Obtains a <see cref="FileWriter"/> that directs output to the current
  /// <see cref="DefaultFileName"/>.
  /// </summary>
  /// <returns>
  /// A <see cref="FileWriter"/> outputting to a file at
  /// <see cref="DefaultFileName"/>.
  /// </returns>
  /// <seealso cref="Instance(string)"/>
  /// <seealso cref="DefaultFileName"/>
  public static FileWriter Instance() {
    lock (_singletonLock) {
      return Instance(DefaultFileName);
    }
  }

  private readonly object _writingLock = new();

  /// <summary>
  /// The path of the filename this Writer is writing to.
  /// </summary>
  public string FileName { get; }

  private FileWriter(string fileName) {
    FileName = fileName;
    lock (_writingLock) {
      // Clear the file
      using var sw = new StreamWriter(FileName);
    }
  }

  [SuppressMessage("Style",
    "IDE0063:Use simple 'using' statement",
    Justification = "Prefer block, to explicitly delineate scope")]
  private void WriteLine(string message) {
    lock (_writingLock) {
      using (var sw = File.AppendText(FileName)) {
        sw.WriteLine(message);
      }
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
