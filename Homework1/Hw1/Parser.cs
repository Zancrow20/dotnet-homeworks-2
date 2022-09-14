

namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args, 
        out double val1, 
        out CalculatorOperation operation, 
        out double val2)
    {
        if (!IsArgLengthSupported(args))
            throw new ArgumentException("Incorrect length:", nameof(args));
        if (!Double.TryParse(args[0], out val1))
            throw new ArgumentException("Incorrect first value:", nameof(val1));
        if (!Double.TryParse(args[2], out val2))
            throw new ArgumentException("Incorrect second value:", nameof(val2));
        operation = ParseOperation(args[1]);
    }

    private static bool IsArgLengthSupported(string[] args) => args.Length == 3;

    private static CalculatorOperation ParseOperation(string arg)
    {
        return arg switch
        {
            "+" => CalculatorOperation.Plus,
            "-" => CalculatorOperation.Minus,
            "*" => CalculatorOperation.Multiply, 
            "/" => CalculatorOperation.Divide,
            _=> throw new InvalidOperationException()
        };
    }
}