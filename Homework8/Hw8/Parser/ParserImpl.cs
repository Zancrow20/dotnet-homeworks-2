using System.Globalization;
using Hw8.Calculator;
using static Hw8.Calculator.Messages;

namespace Hw8.Parser;

public class ParserImpl : IParser
{
    public string TryParseValues(string? firstValue, string? operation, string? secondValue, out Result result)
    {
        result = new Result();
        if (firstValue == null || operation == null || secondValue == null)
            return "Введите достаточное количество аргументов";
        if (!Double.TryParse(firstValue, NumberStyles.AllowDecimalPoint,NumberFormatInfo.InvariantInfo, out var val1) 
            || !Double.TryParse(secondValue,NumberStyles.AllowDecimalPoint,NumberFormatInfo.InvariantInfo, out var val2))
            return InvalidNumberMessage;
        if (!Enum.TryParse<Operation>(operation, out var op) || op == Operation.Invalid)
            return InvalidOperationMessage;
        if (val2 == 0 && op == Operation.Divide)
            return DivisionByZeroMessage;
        result = new Result {firstValue = val1, operation = op, secondValue = val2};
        return OK;
    }
}