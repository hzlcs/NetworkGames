using System.Reflection;

namespace HttpAutoMap.Abstraction;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
public class HttpControllerAttribute : Attribute
{
}