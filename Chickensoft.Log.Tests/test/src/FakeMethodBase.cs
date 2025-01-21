namespace Chickensoft.Log.Tests;

using System;
using System.Globalization;
using System.Reflection;

internal sealed class FakeMethodBase : MethodBase {
#pragma warning disable IDE0052
  private readonly string? _fileName;
#pragma warning restore IDE0052
  private readonly string? _methodName;
  private readonly string? _className;

  public FakeMethodBase(
    string? fileName, string? className, string? methodName
  ) {
    _fileName = fileName;
    _methodName = methodName;
    _className = className;
  }

  public override Type? DeclaringType
    => _className != null ? new FakeType(_className) : null;
  public override string Name => _methodName ?? null!;

  public override MethodAttributes Attributes
    => throw new NotImplementedException();
  public override RuntimeMethodHandle MethodHandle
    => throw new NotImplementedException();
  public override MemberTypes MemberType
    => throw new NotImplementedException();
  public override Type ReflectedType
    => throw new NotImplementedException();
  public override object[] GetCustomAttributes(bool inherit)
    => throw new NotImplementedException();
  public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    => throw new NotImplementedException();
  public override MethodImplAttributes GetMethodImplementationFlags()
    => throw new NotImplementedException();
  public override ParameterInfo[] GetParameters()
    => throw new NotImplementedException();
  public override object? Invoke(
    object? obj,
    BindingFlags invokeAttr,
    Binder? binder,
    object?[]? parameters,
    CultureInfo? culture
  ) => throw new NotImplementedException();
  public override bool IsDefined(Type attributeType, bool inherit)
    => throw new NotImplementedException();
}
