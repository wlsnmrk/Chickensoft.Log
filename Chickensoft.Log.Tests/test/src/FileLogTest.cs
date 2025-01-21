namespace Chickensoft.Log.Tests;

using Chickensoft.Log;
using LightMock;
using LightMock.Generator;
using Shouldly;

public class FileLogTest {
  private readonly string _testMsg = "A test message";

  [Fact]
  public void Initializes() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    log.ShouldBeAssignableTo<ILog>();
  }

  [Fact]
  public void PrintsMessage() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    log.Print(_testMsg);
    mockWriter.Assert(writer =>
        writer.WriteMessage($"{nameof(FileLogTest)}: {_testMsg}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsError() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    log.Err(_testMsg);
    mockWriter.Assert(writer =>
        writer.WriteError($"ERROR in {nameof(FileLogTest)}: {_testMsg}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsWarning() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    log.Warn(_testMsg);
    mockWriter.Assert(writer =>
        writer.WriteWarning($"WARNING in {nameof(FileLogTest)}: {_testMsg}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsException() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    var e = new TestException(_testMsg);
    log.Print(e);
    mockWriter.Assert(writer =>
        writer.WriteError($"ERROR in {nameof(FileLogTest)}: An error occurred."),
      Invoked.Once);
    mockWriter.Assert(writer =>
        writer.WriteError($"ERROR in {nameof(FileLogTest)}: {e}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsStackTrace() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    log.Print(st);
    mockWriter.Assert(static writer =>
        writer.WriteError($"ERROR in {nameof(FileLogTest)}: ClassName.MethodName in File.cs(1,2)"),
      Invoked.Once
    );
  }
}
