namespace Chickensoft.Log.Tests;

using Chickensoft.Log;
using LightMock;
using LightMock.Generator;
using Shouldly;

public class FileLogTest {
  private const string TEST_MSG = "A test message";

  private static string Format(string msg) {
    return $"MockLevel ({nameof(FileLogTest)}): {msg}";
  }

  [Fact]
  public void Initializes() {
    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object);
    log.ShouldBeAssignableTo<ILog>();
  }

  [Fact]
  public void PrintsMessage() {
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(FileLogTest), TEST_MSG))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Print(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(FileLogTest), TEST_MSG),
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
        formatter.FormatError(nameof(FileLogTest), TEST_MSG))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Err(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(FileLogTest), TEST_MSG),
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
        formatter.FormatWarning(nameof(FileLogTest), TEST_MSG))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Warn(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatWarning(nameof(FileLogTest), TEST_MSG),
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
        formatter.FormatError(nameof(FileLogTest), "Exception:"))
      .Returns(formattedExceptionMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(FileLogTest), eMsg))
      .Returns(formattedException);

    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    log.Print(e);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(FileLogTest), "Exception:"),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(FileLogTest), eMsg),
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
        formatter.FormatMessage(nameof(FileLogTest), expectedStackTraceMsg))
      .Returns(formattedStackTraceMsg);

    var mockWriter = new Mock<FileLog.IWriter>();
    var log = new FileLog(nameof(FileLogTest), mockWriter.Object) {
      Formatter = mockFormatter.Object
    };
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    log.Print(st);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(FileLogTest), expectedStackTraceMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedStackTraceMsg),
      Invoked.Once
    );
  }
}
