namespace Chickensoft.Log.Tests;

using LightMock;
using LightMock.Generator;
using Shouldly;

public class ConsoleLogTest {
  private const string TEST_MSG = "A test message";

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
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), TEST_MSG))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Print(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), TEST_MSG),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedTestMsg),
      Invoked.Once);
  }

  [Fact]
  public void PrintsError() {
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), TEST_MSG))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Err(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), TEST_MSG),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteError(formattedTestMsg),
      Invoked.Once);
  }

  [Fact]
  public void PrintsWarning() {
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatWarning(nameof(ConsoleLogTest), TEST_MSG))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Warn(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatWarning(nameof(ConsoleLogTest), TEST_MSG),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteWarning(formattedTestMsg),
      Invoked.Once);
  }

  [Fact]
  public void PrintsException() {
    var e = new TestException(TEST_MSG);
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
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Style",
      "CA1859:Change type of variable for performance",
      Justification = "Need ILog type to test interface method")]
  public void PrintsExceptionWithMessage() {
    var contextMsg = "Context message";
    var exceptionMsg = "Exception:";
    var e = new TestException(TEST_MSG);
    var eStr = e.ToString();
    var formattedContextMsg = Format(contextMsg);
    var formattedExceptionMsg = Format(exceptionMsg);
    var formattedException = Format(eStr);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), contextMsg))
      .Returns(formattedContextMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), exceptionMsg))
      .Returns(formattedExceptionMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), eStr))
      .Returns(formattedException);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = (ILog)new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Print(e, contextMsg);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), contextMsg),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), exceptionMsg),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(ConsoleLogTest), eStr),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteError(formattedContextMsg),
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

  [Fact]
  public void PrintsStackTraceWithoutFile() {
    var expectedStackTraceMsg = "ClassName.MethodName in **(1,2)";
    var formattedStackTraceMsg = Format(expectedStackTraceMsg);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), expectedStackTraceMsg))
      .Returns(formattedStackTraceMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    var st = new FakeStackTrace(null, "ClassName", "MethodName");
    log.Print(st);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), expectedStackTraceMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedStackTraceMsg),
      Invoked.Once
    );
  }

  [Fact]
  public void PrintsStackTraceWithoutClass() {
    var expectedStackTraceMsg = "UnknownClass.MethodName in File.cs(1,2)";
    var formattedStackTraceMsg = Format(expectedStackTraceMsg);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), expectedStackTraceMsg))
      .Returns(formattedStackTraceMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    var st = new FakeStackTrace("File.cs", null, "MethodName");
    log.Print(st);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), expectedStackTraceMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedStackTraceMsg),
      Invoked.Once
    );
  }

  [Fact]
  public void PrintsStackTraceWithoutMethod() {
    // unknown method is also unknown class
    var expectedStackTraceMsg = "UnknownClass.UnknownMethod in File.cs(1,2)";
    var formattedStackTraceMsg = Format(expectedStackTraceMsg);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(ConsoleLogTest), expectedStackTraceMsg))
      .Returns(formattedStackTraceMsg);

    var mockWriter = new Mock<ConsoleLog.IWriter>();
    var log = new ConsoleLog(nameof(ConsoleLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    var st = new FakeStackTrace("File.cs", "ClassName", null);
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
