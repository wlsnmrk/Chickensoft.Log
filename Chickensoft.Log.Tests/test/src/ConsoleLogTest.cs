namespace Chickensoft.Log.Tests;

using LightMock;
using LightMock.Generator;
using Shouldly;

public class ConsoleLogTest {
  private readonly string _testMsg = "A test message";

  [Fact]
  public void Initializes() {
    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object);
    log.ShouldBeAssignableTo<ILog>();
  }

  [Fact]
  public void PrintsMessage() {
    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object);
    log.Print(_testMsg);
    mockWriter.Assert(writer =>
        writer.WriteMessage($"{nameof(ConsoleLogTest)}: {_testMsg}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsError() {
    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object);
    log.Err(_testMsg);
    mockWriter.Assert(writer =>
        writer.WriteError($"{nameof(ConsoleLogTest)}: {_testMsg}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsWarning() {
    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object);
    log.Warn(_testMsg);
    mockWriter.Assert(writer =>
        writer.WriteWarning($"WARNING in {nameof(ConsoleLogTest)}: {_testMsg}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsException() {
    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object);
    var e = new TestException(_testMsg);
    log.Print(e);
    mockWriter.Assert(writer =>
        writer.WriteError($"{nameof(ConsoleLogTest)}: An error occurred."),
      Invoked.Once);
    mockWriter.Assert(writer =>
        writer.WriteError($"{nameof(ConsoleLogTest)}: {e}"),
      Invoked.Once);
  }

  [Fact]
  public void PrintsStackTrace() {
    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object);
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    log.Print(st);
    mockWriter.Assert(static writer =>
        writer.WriteError($"{nameof(ConsoleLogTest)}: ClassName.MethodName in File.cs(1,2)"),
      Invoked.Once
    );
  }
}
