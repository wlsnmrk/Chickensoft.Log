namespace Chickensoft.Log.Tests;

using System.Collections.Generic;
using Shouldly;

public class TestWriterTest
{
  private const string TEST_MSG1 = "A test message 1";
  private const string TEST_MSG2 = "A test message 2";

  [Fact]
  public void WriteMessageStoresMessage()
  {
    var writer = new TestWriter();
    writer.WriteMessage(TEST_MSG1);
    writer.WriteMessage(TEST_MSG2);
    writer.LoggedMessages
      .ShouldBeEquivalentTo(new List<string> { TEST_MSG1, TEST_MSG2 });
    writer.LoggedWarnings.ShouldBeEmpty();
    writer.LoggedErrors.ShouldBeEmpty();
  }

  [Fact]
  public void WriteWarningStoresWarning()
  {
    var writer = new TestWriter();
    writer.WriteWarning(TEST_MSG1);
    writer.WriteWarning(TEST_MSG2);
    writer.LoggedMessages.ShouldBeEmpty();
    writer.LoggedWarnings
      .ShouldBeEquivalentTo(new List<string> { TEST_MSG1, TEST_MSG2 });
    writer.LoggedErrors.ShouldBeEmpty();
  }

  [Fact]
  public void WriteErrorStoresMessage()
  {
    var writer = new TestWriter();
    writer.WriteError(TEST_MSG1);
    writer.WriteError(TEST_MSG2);
    writer.LoggedMessages.ShouldBeEmpty();
    writer.LoggedWarnings.ShouldBeEmpty();
    writer.LoggedErrors
      .ShouldBeEquivalentTo(new List<string> { TEST_MSG1, TEST_MSG2 });
  }

  [Fact]
  public void ResetClearsAllStored()
  {
    var writer = new TestWriter();
    writer.WriteMessage(TEST_MSG1);
    writer.WriteMessage(TEST_MSG2);
    writer.WriteWarning(TEST_MSG1);
    writer.WriteWarning(TEST_MSG2);
    writer.WriteError(TEST_MSG1);
    writer.WriteError(TEST_MSG2);
    writer.Reset();
    writer.LoggedMessages.ShouldBeEmpty();
    writer.LoggedWarnings.ShouldBeEmpty();
    writer.LoggedErrors.ShouldBeEmpty();
  }
}
