# Chickensoft.Log

[![Chickensoft Badge][chickensoft-badge]][chickensoft-website]
[![Discord][discord-badge]][discord]
[![Read the docs][read-the-docs-badge]][docs]
![line coverage][line-coverage]
![branch coverage][branch-coverage]

Opinionated logging interface and implementations for C# applications
and libraries.

---

<p align="center">
<img alt="Chickensoft.Log" src="Chickensoft.Log/icon.png" width="200">
</p>

## 🥚 Getting Started

> [!TIP]
> For logging in Godot, see [Chickensoft.Log.Godot][log-godot].

Install the latest version of the [Chickensoft.Log] package from nuget:

```xml
<PackageReference Include="Chickensoft.Log" Version=... />
```

## 🪵 Usage

### Essentials

In Chickensoft.Log, messages are logged through the `ILog` interface. Each `ILog`
has a name (often the name of the class using the `ILog`), an `ILogFormatter` for
formatting messages, and a collection of `ILogWriter`s, each responsible for
directing log messages to one output.

### Setting up a Log

The package includes a standard implementation of `ILog`, named `Log`. To create
a log, instantiate a `Log` and give it a name and at least one writer.

Example:

```csharp
public class MyClass
{
  // Create a log with the name of MyClass, outputting to stdout/stderr
  private ILog _log = new Log(nameof(MyClass), [new ConsoleWriter()]);
}
```

### Logging

To log messages, use `ILog`'s methods `Print()`, `Warn()`, and `Err()`.

Example:

```csharp
public class MyClass
{
  public void MyMethod()
  {
    // Outputs "Info (MyClass): A log message"
    _log.Print("A log message");
    // Outputs "Warn (MyClass): A warning message"
    _log.Warn("A warning message");
    // Outputs "Error (MyClass): An error occurred"
    _log.Err("An error occurred");

    try
    {
      SomethingThatThrows();
    }
    catch (Exception e)
    {
      // Outputs the value of e.ToString(), prefixed by a line labeling it an exception,
      // as an error
      _log.Print(e);
    }

    // Outputs the current stack trace as a standard log message
    _log.Print(new System.Diagnostics.StackTrace());
  }
}
```

> [!TIP]
> Some writers may have separate channels for warnings and errors, while others
> may not. For instance, the `TraceWriter` has separate channels for regular log
> messages, warnings, and errors. The `FileWriter` has only one channel, the
> file it's writing to. Warnings and errors can still be distinguished by the
> label the formatter gives them.

### Formatting

Optionally, when constructing a log, you can provide an `ILogFormatter` that the
log will use to format each log message. (The formatted message should include
the log's name, the level of the message, and the message itself.)

```csharp
public class MyClass
{
  private ILog _log = new Log(nameof(MyClass), [ConsoleWriter()])
  {
    Formatter = new MyFormatter()
  };
}
```

By default, `Log` will use the included `LogFormatter` class implementing
`ILogFormatter`.

Messages are formatted with one of three level labels, depending which log method
you call. By default, the included `LogFormatter` uses the labels `"Info"`,
`"Warn"`, and `"Error"`. You can change these labels for an individual `LogFormatter`:

```csharp
var formatter = new LogFormatter()
{
  MessagePrefix = "INFO";
  WarningPrefix = "WARN";
  ErrorPrefix = "ERROR";
};
```

You can also change the default values of these labels for all `LogFormatter`s:

```csharp
LogFormatter.DefaultMessagePrefix = "INFO";
LogFormatter.DefaultWarningPrefix = "WARN";
LogFormatter.DefaultErrorPrefix = "ERROR";
```

> [!WARNING]
> Changing the default values of the level labels will affect newly-created
> `LogFormatter`s, but will not affect ones that already exist.

## ✒️ Writer Types

`Log` accepts a list of writers, which receive formatted messages from the log
and are responsible for handling the output of the messages. The Log package
provides three writer types for use in applications or libraries:

* `ConsoleWriter`: Outputs log messages to stdout and stderr.
* `TraceWriter`: Outputs log messages to .NET's `Trace` system. This is useful
for seeing log output in Visual Studio's "Output" tab while debugging.
* `FileWriter`: Outputs log messages to file. By default, `FileWriter` will
write to a file called "output.log" in the working directory, but you can either
configure a different default, or configure individual `FileWriter`s to write to
particular files on creation. To avoid concurrency issues, `FileWriter` is
implemented as a pseudo-singleton with a single instance per file name; see
below for details.

The package provides one additional writer type, `TestWriter`, which may be
useful for testing your code without mocking `ILog` (see below).

### Using `FileWriter`

`FileWriter` provides two static `Instance()` methods for obtaining references
to writers.

You can obtain a reference to a writer using the default file name `"output.log"`:

```csharp
public class MyClass
{
  private ILog _log = new Log(nameof(MyClass), [FileWriter.Instance()]);
}
```

---
You can obtain a writer that outputs messages to a custom file name:

```csharp
public class MyClass
{
  private ILog _log = new Log(nameof(MyClass), [FileWriter.Instance("CustomFileName.log")];
}
```

---
And you can change the default file name for `FileWriter`s:

```csharp
public class Entry
{
  public static void Main()
  {
    // Change the default file name for FileWriter before any writers are created
    FileWriter.DefaultFileName = "MyFileName.log";
    // ...
  }
}

public class MyClass
{
  // Create a FileWriter that writes to the new default name
  private ILog _log = new Log(nameof(MyClass), [FileWriter.Instance()]);
}
```

> [!WARNING]
> Changing the default value for the log file name will affect newly-created
> `FileWriter`s, but will not affect ones that already exist.

### Using `TestWriter`

When testing code that uses an `ILog`, it may be cumbersome to mock `ILog`'s
methods. In that case, you may prefer to use the provided `TestWriter` type,
which accumulates log messages for testing:

```csharp
// Class under test
public class MyClass
{
  public ILog Log { get; set; } = new Log(nameof(MyClass), [new ConsoleWriter()]);

  // Method that logs some information; we want to test the logged messages
  public void MyMethod()
  {
    Log.Print("A normal log message");
    Log.Err("An error message");
  }
}

public class MyClassTest
{
  [Fact]
  public void MyMethodLogs()
  {
    // set up an instance of MyClass, but with a TestWriter instead of a ConsoleWriter
    var testWriter = new TestWriter();
    var obj = new MyClass() { Log = new Log(nameof(MyClass), [testWriter]) };
    obj.MyMethod();
    // use TestWriter to test the logging behavior of MyClass
    testWriter.LoggedMessages
      .ShouldBeEquivalentTo(new List<string>
        {
          "Info (MyClass): A normal log message",
          "Error (MyClass): An error message"
        });
  }
}
```

> [!WARNING]
> `TestWriter` is not thread-safe.

## 💁 Getting Help

*Having issues?* We'll be happy to help you in the [Chickensoft Discord server][discord].

---

🐣 Package generated from a 🐤 Chickensoft Template — <https://chickensoft.games>

[chickensoft-badge]: https://raw.githubusercontent.com/chickensoft-games/chickensoft_site/main/static/img/badges/chickensoft_badge.svg
[chickensoft-website]: https://chickensoft.games
[discord-badge]: https://raw.githubusercontent.com/chickensoft-games/chickensoft_site/main/static/img/badges/discord_badge.svg
[discord]: https://discord.gg/gSjaPgMmYW
[read-the-docs-badge]: https://raw.githubusercontent.com/chickensoft-games/chickensoft_site/main/static/img/badges/read_the_docs_badge.svg
[docs]: https://chickensoft.games/docsickensoft%20Discord-%237289DA.svg?style=flat&logo=discord&logoColor=white
[line-coverage]: Chickensoft.Log.Tests/badges/line_coverage.svg
[branch-coverage]: Chickensoft.Log.Tests/badges/branch_coverage.svg

[Chickensoft.Log]: https://www.nuget.org/packages/Chickensoft.Log
[log-godot]: https://github.com/chickensoft-games/Log.Godot
