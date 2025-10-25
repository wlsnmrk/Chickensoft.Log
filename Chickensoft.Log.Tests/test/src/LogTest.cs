namespace Chickensoft.Log.Tests;

using LightMock;
using LightMock.Generator;
using Shouldly;

public class LogTest
{
  private const string TEST_MSG = "A test message";

  private static string Format(string msg)
    => $"MockLevel ({nameof(LogTest)}): {msg}";

  [Fact]
  public void Initializes()
  {
    var mockWriter = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest), [mockWriter.Object]);
    log.ShouldBeAssignableTo<ILog>();
    log.Name.ShouldBe(nameof(LogTest));
  }

  [Fact]
  public void InitializesWithDefaults()
  {
    var mockWriter = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest));
    log.ShouldBeAssignableTo<ILog>();
    log.Name.ShouldBe(nameof(LogTest));
  }

  [Fact]
  public void WritesPrintMessage()
  {
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(LogTest), TEST_MSG))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest), [mockWriter.Object])
    {
      Formatter = mockFormatter.Object
    };
    log.Print(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(LogTest), TEST_MSG),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedTestMsg),
      Invoked.Once);
  }

  [Fact]
  public void WritesErrorMessage()
  {
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), TEST_MSG))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest), [mockWriter.Object])
    {
      Formatter = mockFormatter.Object
    };
    log.Err(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), TEST_MSG),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteError(formattedTestMsg),
      Invoked.Once);
  }

  [Fact]
  public void WritesWarningMessage()
  {
    var formattedTestMsg = Format(TEST_MSG);
    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatWarning(nameof(LogTest), TEST_MSG))
      .Returns(formattedTestMsg);

    var mockWriter = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest), [mockWriter.Object])
    {
      Formatter = mockFormatter.Object
    };
    log.Warn(TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatWarning(nameof(LogTest), TEST_MSG),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteWarning(formattedTestMsg),
      Invoked.Once);
  }

  [Fact]
  public void WritesExceptionError()
  {
    var e = new TestException(TEST_MSG);
    var eMsg = e.ToString();
    var formattedExceptionMsg = Format("Exception:");
    var formattedException = Format(eMsg);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), "Exception:"))
      .Returns(formattedExceptionMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), eMsg))
      .Returns(formattedException);

    var mockWriter = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest), [mockWriter.Object])
    {
      Formatter = mockFormatter.Object
    };
    log.Err(e);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), "Exception:"),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), eMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteError(formattedExceptionMsg),
      Invoked.Once);
    mockWriter.Assert(writer =>
        writer.WriteError(formattedException),
      Invoked.Once);
  }

  [Fact]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
    "IDE0079:Remove unnecessary suppression",
    Justification = "Compiler will complain about CA1859 even if analyzers don't")]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Style",
    "CA1859:Change type of variable for performance",
    Justification = "Need ILog type to test interface method")]
  public void WritesExceptionWithMessageError()
  {
    var contextMsg = "Context message";
    var exceptionMsg = "Exception:";
    var e = new TestException(TEST_MSG);
    var eStr = e.ToString();
    var formattedContextMsg = Format(contextMsg);
    var formattedExceptionMsg = Format(exceptionMsg);
    var formattedException = Format(eStr);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), contextMsg))
      .Returns(formattedContextMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), exceptionMsg))
      .Returns(formattedExceptionMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), eStr))
      .Returns(formattedException);

    var mockWriter = new Mock<ILogWriter>();
    var log = (ILog)new Log(nameof(LogTest), [mockWriter.Object])
    {
      Formatter = mockFormatter.Object
    };
    log.Err(e, contextMsg);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), contextMsg),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), exceptionMsg),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), eStr),
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
  public void WritesExceptionPrintAsError()
  {
    var e = new TestException(TEST_MSG);
    var eMsg = e.ToString();
    var formattedExceptionMsg = Format("Exception:");
    var formattedException = Format(eMsg);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), "Exception:"))
      .Returns(formattedExceptionMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), eMsg))
      .Returns(formattedException);

    var mockWriter = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest), [mockWriter.Object])
    {
      Formatter = mockFormatter.Object
    };
    log.Print(e);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), "Exception:"),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), eMsg),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteError(formattedExceptionMsg),
      Invoked.Once);
    mockWriter.Assert(writer =>
        writer.WriteError(formattedException),
      Invoked.Once);
  }

  [Fact]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
    "IDE0079:Remove unnecessary suppression",
    Justification = "Compiler will complain about CA1859 even if analyzers don't")]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Style",
    "CA1859:Change type of variable for performance",
    Justification = "Need ILog type to test interface method")]
  public void WritesExceptionWithMessagePrintAsError()
  {
    var contextMsg = "Context message";
    var exceptionMsg = "Exception:";
    var e = new TestException(TEST_MSG);
    var eStr = e.ToString();
    var formattedContextMsg = Format(contextMsg);
    var formattedExceptionMsg = Format(exceptionMsg);
    var formattedException = Format(eStr);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), contextMsg))
      .Returns(formattedContextMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), exceptionMsg))
      .Returns(formattedExceptionMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatError(nameof(LogTest), eStr))
      .Returns(formattedException);

    var mockWriter = new Mock<ILogWriter>();
    var log = (ILog)new Log(nameof(LogTest), [mockWriter.Object])
    {
      Formatter = mockFormatter.Object
    };
    log.Print(e, contextMsg);

    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), contextMsg),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), exceptionMsg),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatError(nameof(LogTest), eStr),
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
  public void WritesStackTrace()
  {
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    var formattedStackTraceMsg = Format("ClassName.MethodName in File.cs(1,2)");

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(LogTest), st))
      .Returns(formattedStackTraceMsg);

    var mockWriter = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest), [mockWriter.Object])
    {
      Formatter = mockFormatter.Object
    };
    log.Print(st);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(LogTest), st),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedStackTraceMsg),
      Invoked.Once
    );
  }

  [Fact]
  public void WritesStackTraceWithMessage()
  {
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    var formattedStackTraceMsg = Format("ClassName.MethodName in File.cs(1,2)");
    var formattedContextMsg = Format(TEST_MSG);

    var mockFormatter = new Mock<ILogFormatter>();
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(LogTest), TEST_MSG))
      .Returns(formattedContextMsg);
    mockFormatter.Arrange(formatter =>
        formatter.FormatMessage(nameof(LogTest), st))
      .Returns(formattedStackTraceMsg);

    var mockWriter = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest), [mockWriter.Object])
    {
      Formatter = mockFormatter.Object
    };
    log.Print(st, TEST_MSG);

    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(LogTest), TEST_MSG),
      Invoked.Once);
    mockFormatter.Assert(formatter =>
        formatter.FormatMessage(nameof(LogTest), st),
      Invoked.Once);

    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedContextMsg),
      Invoked.Once
    );
    mockWriter.Assert(writer =>
        writer.WriteMessage(formattedStackTraceMsg),
      Invoked.Once
    );
  }

  [Fact]
  public void AddsAndRemovesWriters()
  {
    var writerA = new Mock<ILogWriter>();
    var writerB = new Mock<ILogWriter>();
    var log = new Log(nameof(LogTest), [writerA.Object]);

    log.AddWriter(writerB.Object);

    log._writers.ShouldBe([writerA.Object, writerB.Object]);

    log.RemoveWriter(writerA.Object);

    log._writers.ShouldBe([writerB.Object]);
  }
}
