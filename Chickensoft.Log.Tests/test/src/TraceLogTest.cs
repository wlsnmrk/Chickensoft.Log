namespace Chickensoft.Log.Tests;

using Moq;
using Shouldly;

public class TraceLogTest {
  private readonly string _testMsg = "A test message";

  [Fact]
  public void Initializes() {
    var mockWriter = new Mock<TraceLog.IWriter>();
    var log = new TraceLog(nameof(TraceLogTest), mockWriter.Object);
    log.ShouldBeAssignableTo<ILog>();
  }

  [Fact]
  public void PrintsMessage() {
    var mockWriter = new Mock<TraceLog.IWriter>();
    var log = new TraceLog(nameof(TraceLogTest), mockWriter.Object);
    log.Print(_testMsg);
    mockWriter.Verify(writer =>
        writer.WriteMessage($"{nameof(TraceLogTest)}: {_testMsg}"),
      Times.Once());
  }

  [Fact]
  public void PrintsError() {
    var mockWriter = new Mock<TraceLog.IWriter>();
    var log = new TraceLog(nameof(TraceLogTest), mockWriter.Object);
    log.Err(_testMsg);
    mockWriter.Verify(writer =>
        writer.WriteError($"{nameof(TraceLogTest)}: {_testMsg}"),
      Times.Once());
  }

  [Fact]
  public void PrintsWarning() {
    var mockWriter = new Mock<TraceLog.IWriter>();
    var log = new TraceLog(nameof(TraceLogTest), mockWriter.Object);
    log.Warn(_testMsg);
    mockWriter.Verify(writer =>
        writer.WriteWarning($"WARNING in {nameof(TraceLogTest)}: {_testMsg}"),
      Times.Once());
  }

  [Fact]
  public void PrintsException() {
    var mockWriter = new Mock<TraceLog.IWriter>();
    var log = new TraceLog(nameof(TraceLogTest), mockWriter.Object);
    var e = new TestException(_testMsg);
    log.Print(e);
    mockWriter.Verify(writer =>
        writer.WriteError($"{nameof(TraceLogTest)}: An error occurred."),
      Times.Once());
    mockWriter.Verify(writer =>
        writer.WriteError($"{nameof(TraceLogTest)}: {e}"),
      Times.Once());
  }
}
