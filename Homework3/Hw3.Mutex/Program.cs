using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Hw3.Mutex;

public class Program
{
    [ExcludeFromCodeCoverage]
    static void Main(string[] args)
    {
        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {Process.GetCurrentProcess().Id} starts");
        using var wm = new WithMutex();
        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {Process.GetCurrentProcess().Id} acquires mutex");
        Console.WriteLine(wm.Increment());
        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} {Process.GetCurrentProcess().Id} releases mutex");
    }
}