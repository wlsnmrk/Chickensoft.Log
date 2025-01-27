namespace Chickensoft.Log.Tests;

using LightMock;
using LightMock.Generator;
using Shouldly;

public class ConsoleLogTest {
  private static readonly string _testMsg = "A test message";

  private static string Format(string msg) {
    return $"MockLevel ({nameof(ConsoleLogTest)}): {msg}";
  }

  [Fact]
  public void Initializes() {
    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object);
    log.ShouldBeAssignableTo<ILog>();
  }

  [Fact]
  public void PrintsMessage() {
    var formattedTestMsg = Format(_testMsg);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), _testMsg))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Print(_testMsg);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), _testMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedTestMsg),
      Invoked.Once);
  }

  [Fact]
  public void PrintsError() {
    var formattedTestMsg = Format(_testMsg);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), _testMsg))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Err(_testMsg);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), _testMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteError(formattedTestMsg),
      Invoked.Once);
  }

  [Fact]
  public void PrintsWarning() {
    var formattedTestMsg = Format(_testMsg);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatWarning(nameof(ConsoleLogTest), _testMsg))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Warn(_testMsg);

    mockFormatter.Assert(formatter =>
        formatter.FormatWarning(nameof(ConsoleLogTest), _testMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteWarning(formattedTestMsg),
      Invoked.Once);
  }

  [Fact]
  public void PrintsException() {
    var e = new TestException(_testMsg);
    var eMsg = e.ToString();
    var formattedExceptionMsg = Format("Exception:");
    var formattedException = Format(eMsg);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), "Exception:"))
      .Returns(formattedExceptionMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), eMsg))
      .Returns(formattedException);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Print(e);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), "Exception:"),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), eMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteError(formattedExceptionMsg),
      Invoked.Once);
    mockWriter.Assert(writer =>
        writer.WriteError(formattedException),
      Invoked.Once);
  }

  [Fact]
  public void PrintsStackTrace() {
    var expectedStackTraceMsg = "ClassName.MethodName in File.cs(1,2)";
    var formattedStackTraceMsg = Format(expectedStackTraceMsg);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), expectedStackTraceMsg))
      .Returns(formattedStackTraceMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    log.Print(st);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), expectedStackTraceMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedStackTraceMsg),
      Invoked.Once
    );
  }
}
