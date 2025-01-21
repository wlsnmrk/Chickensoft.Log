namespace Chickensoft.Log.Tests;

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
    var mockLog1 = new Mock<ILog>();
    var mockLog2 = new Mock<ILog>();
    var logs = new List<ILog> { mockLog1.Object, mockLog2.Object };
    var log = new MultiLog(logs);
    log.Print(_testMsg);
    mockLog1.Verify(log => log.Print(_testMsg), Times.Once());
  }

  [Fact]
  public void PrintsError() {
    var mockLog1 = new Mock<ILog>();
    var mockLog2 = new Mock<ILog>();
    var logs = new List<ILog> { mockLog1.Object, mockLog2.Object };
    var log = new MultiLog(logs);
    log.Err(_testMsg);
    mockLog1.Verify(log => log.Err(_testMsg), Times.Once());
  }

  [Fact]
  public void PrintsWarning() {
    var mockLog1 = new Mock<ILog>();
    var mockLog2 = new Mock<ILog>();
    var logs = new List<ILog> { mockLog1.Object, mockLog2.Object };
    var log = new MultiLog(logs);
    log.Warn(_testMsg);
    mockLog1.Verify(log => log.Warn(_testMsg), Times.Once());
  }

  [Fact]
  public void PrintsException() {
    var mockLog1 = new Mock<ILog>();
    var mockLog2 = new Mock<ILog>();
    var logs = new List<ILog> { mockLog1.Object, mockLog2.Object };
    var log = new MultiLog(logs);
    var e = new TestException(_testMsg);
    log.Print(e);
    mockLog1.Verify(log => log.Print(e), Times.Once());
    mockLog2.Verify(log => log.Print(e), Times.Once());
  }
}
