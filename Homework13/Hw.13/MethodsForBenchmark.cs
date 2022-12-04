using System.Diagnostics.CodeAnalysis;

namespace Hw13;

[ExcludeFromCodeCoverage]
public class MethodsForBenchmark
{
    public string Simple(string s) => s + s;
    public virtual string Virtual(string s) => s + s;
    public static string Static(string s) => s + s;
    public string GenericExample<T>(T s) => s.ToString() + s;
    public string Generic(string s) => GenericExample(s);
    public string Dynamic(string s) => (s as dynamic).ToString() + s;
    public string? Reflection(string s) => (string) typeof(MethodsForBenchmark).GetMethod("Simple")?.Invoke(this, new object[]{s})!;
}