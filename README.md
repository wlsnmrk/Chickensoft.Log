# Chickensoft.Log

[![Chickensoft Badge][chickensoft-badge]][chickensoft-website] [![Discord][discord-badge]][discord] [![Read the docs][read-the-docs-badge]][docs] ![line coverage][line-coverage] ![branch coverage][branch-coverage]

Opinionated, simple logging interface and implementations for C#. Forms the basis for [Log.Godot][log-godot].

---

<p align="center">
<img alt="Chickensoft.Log" src="Chickensoft.Log/icon.png" width="200">
</p>

## ⌨️ Getting Started

> [!TIP]
> For logging in Godot, see [Chickensoft.Log.Godot][log-godot].

Install the latest version of the [Chickensoft.Log] package from nuget:

```xml
<PackageReference Include="Chickensoft.Log" Version=... />
```

## 📜 Logging

### Setup

```csharp
public class MyClass
{
  // Create a log that outputs messages to stdout/stderr, prefixed with the name of the class.
  private ILog _log = new ConsoleLog(nameof(MyClass));
}
```

### Output

```csharp
public void MyMethod()
{
  _log.Print("A log message");
  _log.Warn("A warning message");
  _log.Err("An error occurred");
  try
  {
    SomethingThatThrows();
  }
  catch (Exception e)
  {
    _log.Print(e);
    // handle exception
  }
}
```

## 🪵 Log Types

The Log package provides four log types implementing the `ILog` interface:

* `ConsoleLog`: Outputs log messages to stdout/stderr.
* `TraceLog`: Outputs log messages to .NET's `Trace` system. This is useful for seeing log output in Visual Studio's "Output" tab while debugging.
* `FileLog`: Outputs log messages to file. All `FileLog`s will write to a file called "output.log" in the working directory by default, but you can either configure a different default, or configure individual `FileLog`s to write to particular files on creation.
* `MultiLog`: Delegates log messages to multiple other logs, allowing you to log the same message to, e.g., stdout/stderr and to file with one method call.

### Using FileLog

Create a log that outputs messages to the default filename ("output.log"):

```csharp
public class MyClass
{
  private ILog _log = new FileLog(nameof(MyClass));
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

---
Create a log that outputs messages to a custom filename:

```csharp
public class MyClass
{
  private ILog _log = new FileLog(nameof(MyClass), "CustomFileName.log");
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
