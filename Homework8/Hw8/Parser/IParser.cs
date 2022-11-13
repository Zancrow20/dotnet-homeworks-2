namespace Hw8.Parser;

public interface IParser
{
    string TryParseValues(string? firstValue, string? operation, string? secondValue, out Result result);
}