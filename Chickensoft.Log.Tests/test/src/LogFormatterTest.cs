namespace Chickensoft.Log.Tests;

using Shouldly;

public class LogFormatterTest {
  private readonly string _testMsg = "A test message";
  private readonly string _testName = "TestName";
  private readonly string _testMessagePrefix = "INFO";
  private readonly string _testWarningPrefix = "WARN";
  private readonly string _testErrorPrefix = "ERROR";

  [Fact]
  public void IsILogFormatter() {
    var formatter = new LogFormatter();
    formatter.ShouldBeAssignableTo<ILogFormatter>();
  }

  [Fact]
  public void FormatsMessage() {
    var formatter = new LogFormatter();
    formatter.FormatMessage(_testName, _testMsg)
      .ShouldBe($"Info ({_testName}): {_testMsg}");
  }

  [Fact]
  public void FormatsStackTrace() {
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    var formatter = new LogFormatter();
    formatter.FormatMessage(_testName, st)
      .ShouldBe($"Info ({_testName}): ClassName.MethodName in File.cs(1,2)");
  }

  [Fact]
  public void FormatsStackTraceWithoutFile() {
    var st = new FakeStackTrace(null, "ClassName", "MethodName");
    var formatter = new LogFormatter();
    formatter.FormatMessage(_testName, st)
      .ShouldBe($"Info ({_testName}): ClassName.MethodName in **(1,2)");
  }

  [Fact]
  public void FormatsStackTraceWithoutClass() {
    var st = new FakeStackTrace("File.cs", null, "MethodName");
    var formatter = new LogFormatter();
    formatter.FormatMessage(_testName, st)
      .ShouldBe($"Info ({_testName}): UnknownClass.MethodName in File.cs(1,2)");
  }

  [Fact]
  public void FormatsStackTraceWithoutMethod() {
    var st = new FakeStackTrace("File.cs", "ClassName", null);
    var formatter = new LogFormatter();
    // unknown method is also unknown class
    formatter.FormatMessage(_testName, st)
      .ShouldBe($"Info ({_testName}): UnknownClass.UnknownMethod in File.cs(1,2)");
  }

  [Fact]
  public void FormatsWarning() {
    var formatter = new LogFormatter();
    formatter.FormatWarning(_testName, _testMsg)
      .ShouldBe($"Warn ({_testName}): {_testMsg}");
  }

  [Fact]
  public void FormatsError() {
    var formatter = new LogFormatter();
    formatter.FormatError(_testName, _testMsg)
      .ShouldBe($"Error ({_testName}): {_testMsg}");
  }

  [Fact]
  public void ChangesMessagePrefix() {
    var formatter = new LogFormatter {
      MessagePrefix = _testMessagePrefix
    };
    formatter.FormatMessage(_testName, _testMsg)
      .ShouldBe($"{_testMessagePrefix} ({_testName}): {_testMsg}");
  }

  [Fact]
  public void ChangesWarningPrefix() {
    var formatter = new LogFormatter {
      WarningPrefix = _testWarningPrefix
    };
    formatter.FormatWarning(_testName, _testMsg)
      .ShouldBe($"{_testWarningPrefix} ({_testName}): {_testMsg}");
  }

  [Fact]
  public void ChangesErrorPrefix() {
    var formatter = new LogFormatter {
      ErrorPrefix = _testErrorPrefix
    };
    formatter.FormatError(_testName, _testMsg)
      .ShouldBe($"{_testErrorPrefix} ({_testName}): {_testMsg}");
  }

  [Fact]
  public void ChangesDefaultMessagePrefix() {
    var originalDefault = LogFormatter.DefaultMessagePrefix;
    LogFormatter.DefaultMessagePrefix = _testMessagePrefix;
    var formatter = new LogFormatter();
    formatter.FormatMessage(_testName, _testMsg)
      .ShouldBe($"{_testMessagePrefix} ({_testName}): {_testMsg}");
    LogFormatter.DefaultMessagePrefix = originalDefault;
  }

  [Fact]
  public void ChangesDefaultWarningPrefix() {
    var originalDefault = LogFormatter.DefaultWarningPrefix;
    LogFormatter.DefaultWarningPrefix = _testWarningPrefix;
    var formatter = new LogFormatter();
    formatter.FormatWarning(_testName, _testMsg)
      .ShouldBe($"{_testWarningPrefix} ({_testName}): {_testMsg}");
    LogFormatter.DefaultWarningPrefix = originalDefault;
  }

  [Fact]
  public void ChangesDefaultErrorPrefix() {
    var originalDefault = LogFormatter.DefaultErrorPrefix;
    LogFormatter.DefaultErrorPrefix = _testErrorPrefix;
    var formatter = new LogFormatter();
    formatter.FormatError(_testName, _testMsg)
      .ShouldBe($"{_testErrorPrefix} ({_testName}): {_testMsg}");
    LogFormatter.DefaultErrorPrefix = originalDefault;
  }
}
