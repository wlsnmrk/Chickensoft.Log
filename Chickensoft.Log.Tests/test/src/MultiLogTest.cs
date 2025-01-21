namespace Chickensoft.Log.Tests;

using System.Collections.Generic;
using System.Linq;
using Moq;
using Shouldly;

public class MultiLogTest {
  private readonly string _testMsg = "A test message";

  [Fact]
  public void Initializes() {
    var log = new MultiLog();
    log.ShouldBeAssignableTo<ILog>();
  }

  [Fact]
  public void PrintsMessage() {
    var mockLogs = new List<Mock<ILog>> { new(), new() };
    var logs = (from ml in mockLogs select ml.Object).ToList();
    var log = new MultiLog(logs);
    log.Print(_testMsg);
    foreach (var ml in mockLogs) {
      ml.Verify(log => log.Print(_testMsg), Times.Once());
    }
  }

  [Fact]
  public void PrintsError() {
    var mockLogs = new List<Mock<ILog>> { new(), new() };
    var logs = (from ml in mockLogs select ml.Object).ToList();
    var log = new MultiLog(logs);
    log.Err(_testMsg);
    foreach (var ml in mockLogs) {
      ml.Verify(log => log.Err(_testMsg), Times.Once());
    }
  }

  [Fact]
  public void PrintsWarning() {
    var mockLogs = new List<Mock<ILog>> { new(), new() };
    var logs = (from ml in mockLogs select ml.Object).ToList();
    var log = new MultiLog(logs);
    log.Warn(_testMsg);
    foreach (var ml in mockLogs) {
      ml.Verify(log => log.Warn(_testMsg), Times.Once());
    }
  }

  [Fact]
  public void PrintsException() {
    var mockLogs = new List<Mock<ILog>> { new(), new() };
    var logs = (from ml in mockLogs select ml.Object).ToList();
    var log = new MultiLog(logs);
    var e = new TestException(_testMsg);
    log.Print(e);
    foreach (var ml in mockLogs) {
      ml.Verify(log => log.Print(e), Times.Once());
    }
  }

  [Fact]
  public void PrintsStackTrace() {
    var mockLogs = new List<Mock<ILog>> { new(), new() };
    var logs = (from ml in mockLogs select ml.Object).ToList();
    var log = new MultiLog(logs);
    var st = new FakeStackTrace("File.cs", "ClassName", "MethodName");
    log.Print(st);
    foreach (var ml in mockLogs) {
      ml.Verify(log => log.Print(st), Times.Once());
    }
  }
}
