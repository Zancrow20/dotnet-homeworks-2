using BenchmarkDotNet.Running;
using Hw12;
using StackExchange.Profiling;

var profiler = MiniProfiler.StartNew("Calculator");
using (profiler.Step("Main Work"))
{
    BenchmarkRunner.Run<WebApplicationWorkingTimeTests>();
}
Console.WriteLine(profiler.RenderPlainText());