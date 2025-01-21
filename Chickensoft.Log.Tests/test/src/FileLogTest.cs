namespace Chickensoft.Log.Tests;

using Chickensoft.Log;
using Moq;
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
    mockWriter.Verify(writer =>
        writer.WriteMessage($"{nameof(FileLogTest)}: {_testMsg}"),
      Times.Once());
  }

  [Fact]
  public void PrintsError() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    log.Err(_testMsg);
    mockWriter.Verify(writer =>
        writer.WriteError($"ERROR in {nameof(FileLogTest)}: {_testMsg}"),
      Times.Once());
  }

  [Fact]
  public void PrintsWarning() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    log.Warn(_testMsg);
    mockWriter.Verify(writer =>
        writer.WriteWarning($"WARNING in {nameof(FileLogTest)}: {_testMsg}"),
      Times.Once());
  }

  [Fact]
  public void PrintsException() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    var e = new TestException(_testMsg);
    log.Print(e);
    mockWriter.Verify(writer =>
        writer.WriteError($"ERROR in {nameof(FileLogTest)}: An error occurred."),
      Times.Once());
    mockWriter.Verify(writer =>
        writer.WriteError($"ERROR in {nameof(FileLogTest)}: {e}"),
      Times.Once());
  }
}
