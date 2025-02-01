namespace Chickensoft.Log.Tests;

using System;
using System.IO;
using Shouldly;

public class FileWriterStreamTester : IDisposable {
  private bool _isDisposed;
  private readonly MemoryStream _memoryStream;

  public FileWriterStreamTester(
    string filename = FileWriter.DEFAULT_FILE_NAME
  ) {
    _memoryStream = new MemoryStream();

    FileWriter.AppendText = fileName => new StreamWriter(
      _memoryStream,
      System.Text.Encoding.UTF8,
      bufferSize: 1024,
      leaveOpen: true
    );
  }

  public string GetString() {
    _memoryStream.Position = 0;
    using var reader = new StreamReader(_memoryStream);
    return reader.ReadToEnd();
  }

  public void Dispose() {
    if (_isDisposed) { return; }

    GC.SuppressFinalize(this);
    _isDisposed = true;

    FileWriter.AppendText = FileWriter.AppendTextDefault;
    _memoryStream.Dispose();
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
    var writer = FileWriter.Instance();
    writer.ShouldNotBeNull();
    writer.ShouldBeOfType<FileWriter>();
  }

  [Fact]
  public void NewInstance() {
    var filename = "test.log";
    var writer = FileWriter.Instance(filename);
    writer.ShouldNotBeNull();
    writer.ShouldBeOfType<FileWriter>();
  }

  [Fact]
  public void ReusesInstanceAndRemoves() {
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
