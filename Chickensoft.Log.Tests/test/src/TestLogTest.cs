namespace Chickensoft.Log.Tests;

using System;
using LightMock;
using LightMock.Generator;
using Shouldly;

public class TestLogTest {
  private const string TEST_MSG = "A test message";

  private static string Format(string msg) {
    return $"MockLevel (Test): {msg}";
  }

  [Fact]
  public void Initializes() {
    var log = new TestLog();
    log.ShouldBeAssignableTo<ILog>();
  }

  [Fact]
  public void PrintsMessage() {
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    var log = new TestLog() {
      Formatter = mockFormatter.Object
    };
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(log.Name, TEST_MSG))
      .Returns(formattedTestMsg);
    log.Print(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(log.Name, TEST_MSG),
      Invoked.Once);

    log.LoggedTextBuilder.ToString()
      .ShouldBe(formattedTestMsg + Environment.NewLine);
  }

  [Fact]
  public void PrintsError() {
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    var log = new TestLog() {
      Formatter = mockFormatter.Object
    };
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(log.Name, TEST_MSG))
      .Returns(formattedTestMsg);
    log.Err(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(log.Name, TEST_MSG),
      Invoked.Once);

    log.LoggedTextBuilder.ToString()
      .ShouldBe(formattedTestMsg + Environment.NewLine);
  }

  [Fact]
  public void PrintsWarning() {
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    var log = new TestLog() {
      Formatter = mockFormatter.Object
    };
    mockFormatter.Arrange(formatter =>
        formatter.FormatWarning(log.Name, TEST_MSG))
      .Returns(formattedTestMsg);
    log.Warn(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatWarning(log.Name, TEST_MSG),
      Invoked.Once);

    log.LoggedTextBuilder.ToString()
      .ShouldBe(formattedTestMsg + Environment.NewLine);
  }

  [Fact]
  public void PrintsException() {
    var e = new TestException(TEST_MSG);
    var eMsg = e.ToString();
    var formattedExceptionMsg = Format("Exception:");
    var formattedException = Format(eMsg);

    var mockFormatter = new Mock<ILogFormatter>();
    var log = new TestLog() {
      Formatter = mockFormatter.Object
    };
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(log.Name, "Exception:"))
      .Returns(formattedExceptionMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(log.Name, eMsg))
      .Returns(formattedException);
    log.Print(e);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(log.Name, "Exception:"),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(log.Name, eMsg),
      Invoked.Once);

    log.LoggedTextBuilder.ToString()
      .ShouldBe(formattedExceptionMsg + Environment.NewLine
        + formattedException + Environment.NewLine);
  }

  [Fact]
  public void PrintsStackTrace() {
    var expectedStackTraceMsg = "ClassName.MethodName in File.cs(1,2)";
    var formattedStackTraceMsg = Format(expectedStackTraceMsg);

    var mockFormatter = new Mock<ILogFormatter>();
    var log = new TestLog() {
      Formatter = mockFormatter.Object
    };
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(log.Name, expectedStackTraceMsg))
      .Returns(formattedStackTraceMsg);
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    log.Print(st);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(log.Name, expectedStackTraceMsg),
      Invoked.Once);

    log.LoggedTextBuilder.ToString()
      .ShouldBe(formattedStackTraceMsg + Environment.NewLine);
  }
}
