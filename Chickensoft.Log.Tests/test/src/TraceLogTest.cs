namespace Chickensoft.Log.Tests;

using LightMock;
using LightMock.Generator;
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
    mockWriter.Assert(writer =>
        writer.WriteMessage($"{nameof(TraceLogTest)}: {_testMsg}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsError() {
    var mockWriter = new Mock<TraceLog.IWriter>();
    var log = new TraceLog(nameof(TraceLogTest), mockWriter.Object);
    log.Err(_testMsg);
    mockWriter.Assert(writer =>
        writer.WriteError($"{nameof(TraceLogTest)}: {_testMsg}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsWarning() {
    var mockWriter = new Mock<TraceLog.IWriter>();
    var log = new TraceLog(nameof(TraceLogTest), mockWriter.Object);
    log.Warn(_testMsg);
    mockWriter.Assert(writer =>
        writer.WriteWarning($"WARNING in {nameof(TraceLogTest)}: {_testMsg}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsException() {
    var mockWriter = new Mock<TraceLog.IWriter>();
    var log = new TraceLog(nameof(TraceLogTest), mockWriter.Object);
    var e = new TestException(_testMsg);
    log.Print(e);
    mockWriter.Assert(writer =>
        writer.WriteError($"{nameof(TraceLogTest)}: An error occurred."),
      Invoked.Once);
    mockWriter.Assert(writer =>
        writer.WriteError($"{nameof(TraceLogTest)}: {e}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsStackTrace() {
    var mockWriter = new Mock<TraceLog.IWriter>();
    var log = new TraceLog(nameof(TraceLogTest), mockWriter.Object);
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    log.Print(st);
    mockWriter.Assert(static writer =>
        writer.WriteError($"{nameof(TraceLogTest)}: ClassName.MethodName in File.cs(1,2)"),
      Invoked.Once
    );
  }
}
