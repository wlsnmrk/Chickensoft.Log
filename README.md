# Chickensoft.Log

[![Chickensoft Badge][chickensoft-badge]][chickensoft-website] [![Discord][discord-badge]][discord] [![Read the docs][read-the-docs-badge]][docs] ![line coverage][line-coverage] ![branch coverage][branch-coverage]

Opinionated, simple logging interface and implementations for C# applications and libraries. Forms the basis for [Log.Godot][log-godot].

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

## 📜 Usage

### Setup

```csharp
public class MyClass
{
  // Create a log that outputs messages to stdout/stderr, prefixed with the name of the class.
  private ILog _log = new ConsoleLog(nameof(MyClass));
}
```

Optionally, you can provide an `ILogFormatter` that the log will use to format its name, the level of the log message, and the log message itself. (By default, logs included with the package will use the standard `LogFormatter`.)

```csharp
public class MyClass
{
  private ILog _log = new ConsoleLog(nameof(MyClass))
  {
    Formatter = new MyFormatter()
  };
}
```

### Logging

```csharp
public void MyMethod()
{
  _log.Print("A log message"); // Outputs "Info (MyClass): A log message"
  _log.Warn("A warning message"); // Outputs "Warn (MyClass): A warning message"
  _log.Err("An error occurred"); // Outputs "Error (MyClass): An error occurred"
  try
  {
    SomethingThatThrows();
  }
  catch (Exception e)
  {
    _log.Print(e); // Outputs the value of e.ToString(), prefixed by a line labeling it as an exception
    // ...
  }
}
```

### Formatting

Messages are formatted with one of three level labels, depending which log method you call. By default, the included `LogFormatter` uses the labels `"Info"`, `"Warn"`, and `"Error"`. You can change these labels for an individual `LogFormatter`:

```csharp
var formatter = new LogFormatter();
formatter.MessagePrefix = "INFO";
formatter.WarningPrefix = "WARN";
formatter.ErrorPrefix = "ERROR";
```

You can also change the default values for these labels:

```csharp
LogFormatter.DefaultMessagePrefix = "INFO";
LogFormatter.DefaultWarningPrefix = "WARN";
LogFormatter.DefaultErrorPrefix = "ERROR";
```

> [!WARNING]
> Changing the default values for the level labels will affect newly-created `LogFormatter`s, but will not affect ones that already exist.

## 🪵 Log Types

The Log package provides four operational log types implementing the `ILog` interface:

* `ConsoleLog`: Outputs log messages to stdout/stderr.
* `TraceLog`: Outputs log messages to .NET's `Trace` system. This is useful for seeing log output in Visual Studio's "Output" tab while debugging.
* `FileLog`: Outputs log messages to file. All `FileLog`s will write to a file called "output.log" in the working directory by default, but you can either configure a different default, or configure individual `FileLog`s to write to particular files on creation.
* `MultiLog`: Delegates log messages to multiple other logs, allowing you to log the same message to, e.g., stdout/stderr and to file with one method call.

The package provides one additional, non-operational log type, `TestLog`, which may be useful for testing your code without mocking `ILog`.

### Using `FileLog`

Create a log that outputs messages to the default filename `"output.log"`:

```csharp
public class MyClass
{
  private ILog _log = new FileLog(nameof(MyClass));
}
```

---
Create a log that outputs messages to a custom filename:

```csharp
public class MyClass
{
  private ILog _log = new FileLog(nameof(MyClass), "CustomFileName.log");
}
```

---
Change the default filename for `FileLog`s:

```csharp
public class Entry
{
  public static void Main()
  {
    // Change the default filename for FileLog before any logs are created
    FileLog.Writer.DefaultFileName = "MyFileName.log";
  }
}

public class MyClass
{
  private ILog _log = new FileLog(nameof(MyClass));
}
```

### Using `TestLog`

When testing code that uses an `ILog`, it may be cumbersome to mock `ILog`'s methods. In that case, you may prefer to use the provided `TestLog` type, which accumulates log messages for testing:

```csharp
public class MyClass
{
  public ILog Log { get; set; } = new ConsoleLog();

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
    var obj = new MyClass() { Log = new TestLog() };
    obj.MyMethod();
    obj.Log.LoggedMessages.Count.ShouldBe(2);
    obj.Log.LoggedMessages[0].ShouldBe("Info (Test): A normal log message");
    obj.Log.LoggedMessages[1].ShouldBe("Error (Test): An error message");
  }
}
```

## ✋ Intentional Limitations

The Log package does not provide thread safety. If you are using the Log package in a multithreaded environment, please be sure to employ thread-safe access to your log objects (especially if using multiple `FileLog`s).

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
