namespace Chickensoft.Log.Tests;

using System;
using System.IO;
using Shouldly;

public class FileWriterStreamTester : IDisposable {
  private bool _isDisposed;
  private readonly MemoryStream _openingMemoryStream;
  private readonly MemoryStream _appendingMemoryStream;

  public FileWriterStreamTester(
    string filename = FileWriter.DEFAULT_FILE_NAME
  ) {
    _openingMemoryStream = new MemoryStream();
    _appendingMemoryStream = new MemoryStream();

    FileWriter.AppendText = fileName => new StreamWriter(
      _appendingMemoryStream,
      System.Text.Encoding.UTF8,
      bufferSize: 1024,
      leaveOpen: true
    );

    FileWriter.CreateFile = fileName => _openingMemoryStream;
  }

  public string GetString() {
    _appendingMemoryStream.Position = 0;
    using var reader = new StreamReader(_appendingMemoryStream);
    return reader.ReadToEnd();
  }

  public void Dispose() {
    if (_isDisposed) { return; }

    GC.SuppressFinalize(this);
    _isDisposed = true;

    FileWriter.AppendText = FileWriter.AppendTextDefault;
    FileWriter.CreateFile = FileWriter.CreateFileDefault;
    _openingMemoryStream.Dispose();
    _appendingMemoryStream.Dispose();
  }
}

public class FileWriterTest {
  [Fact]
  public void DefaultFileName() {
    FileWriter.DefaultFileName.ShouldBe(FileWriter.DEFAULT_FILE_NAME);

    var filename = "test.log";
    FileWriter.DefaultFileName = filename;
    FileWriter.DefaultFileName.ShouldBe(filename);

    FileWriter.DefaultFileName = FileWriter.DEFAULT_FILE_NAME;
  }

  [Fact]
  public void DefaultInstance() {
    using var tester = new FileWriterStreamTester();

    var writer = FileWriter.Instance();
    writer.ShouldNotBeNull();
    writer.ShouldBeOfType<FileWriter>();
  }

  [Fact]
  public void NewInstance() {
    using var tester = new FileWriterStreamTester();

    var filename = "test.log";
    var writer = FileWriter.Instance(filename);
    writer.ShouldNotBeNull();
    writer.ShouldBeOfType<FileWriter>();
  }

  [Fact]
  public void ReusesInstanceAndRemoves() {
    using var tester = new FileWriterStreamTester();

    var filename = "test.log";
    var writer1 = FileWriter.Instance(filename);
    var writer2 = FileWriter.Instance(filename);
    writer1.ShouldBeSameAs(writer2);

    FileWriter.Remove(filename).ShouldBeSameAs(writer1);
    FileWriter.Remove(filename).ShouldBeNull();
  }

  [Fact]
  public void WriteMessage() {
    using var tester = new FileWriterStreamTester();

    var writer = FileWriter.Instance();
    var value = "test message";

    writer.WriteMessage(value);

    tester.GetString().ShouldBe(value + Environment.NewLine);
  }

  [Fact]
  public void WriteWarning() {
    using var tester = new FileWriterStreamTester();

    var writer = FileWriter.Instance();
    var value = "test message";

    writer.WriteWarning(value);

    tester.GetString().ShouldBe(value + Environment.NewLine);
  }

  [Fact]
  public void WriteError() {
    using var tester = new FileWriterStreamTester();

    var writer = FileWriter.Instance();
    var value = "test message";

    writer.WriteError(value);

    tester.GetString().ShouldBe(value + Environment.NewLine);
  }
}
