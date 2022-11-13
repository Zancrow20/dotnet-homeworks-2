using Hw8.Calculator;

namespace Hw8.Parser;

public record Result
{
    public double firstValue { get; set; }
    public Operation operation { get; set; }
    public double secondValue { get; set; }
}