using System.Linq.Expressions;
using System.Text.RegularExpressions;
using static Hw10.ErrorMessages.MathErrorMessager;
namespace Hw10.Services.RecursiveParser;

public class RecursiveDescentParser : IRecursiveParser
{
    private readonly string[] _operations = {"+", "-", "*", "/"};
    private int _bracketsCount;
    private readonly string[]? _tokens;
    private int _position;

    private static readonly Regex InputSplit = new("(?<=[−+*/\\(\\)])|(?=[−+*/\\(\\)])");
    private static readonly Regex Numbers = new("[0-9]+");

    public RecursiveDescentParser(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new NullReferenceException(EmptyString);
        _tokens = InputSplit.Split(expression)
            .SelectMany(str => str.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToArray();
    }

    public Expression Parse()
    {
        var result = Expression();
        if (_position == _tokens.Length) return result;
        if (_tokens[_position < _tokens.Length ? _position : ^1] == ")")
            _bracketsCount++;
        var message = _bracketsCount != 0
            ? IncorrectBracketsNumber
            : UnknownCharacterMessage(_tokens![_position].ToCharArray()[0]);
        throw new ArgumentException(message);
    }

    private Expression Expression()
    {
        var firstExpression = Term();
        while (_position < _tokens.Length)
        {
            var token = _tokens[_position];
            if (!token.Equals("+") && !token.Equals("-")) break;
            _position++;
            var secondExpression = Term();
            firstExpression = token.Equals("+")
                ? System.Linq.Expressions.Expression.Add(firstExpression, secondExpression)
                : System.Linq.Expressions.Expression.Subtract(firstExpression, secondExpression);
        }

        return firstExpression;
    }


    private Expression Term()
    {
        var firstExpression = Factor();
        while (_position < _tokens.Length)
        {
            var token = _tokens[_position];
            if (!token.Equals("*") && !token.Equals("/")) break;
            _position++;
            var secondExpression = Factor();
            firstExpression = token.Equals("*")
                ? System.Linq.Expressions.Expression.Multiply(firstExpression, secondExpression)
                : System.Linq.Expressions.Expression.Divide(firstExpression, secondExpression);
        }

        return firstExpression;
    }

    private Expression Factor()
    {
        var next = _position < _tokens.Length ? _tokens[_position] : string.Empty;
        var previous = _position - 1 >= 0 ? _tokens[_position - 1] : string.Empty;
        if (next.Equals("("))
        {
            _bracketsCount++;
            _position++;
            var result = Expression();
            if (_tokens[_position < _tokens.Length ? _position : ^1] != ")")
                throw new ArgumentException(IncorrectBracketsNumber);
            _bracketsCount--;
            _position++;
            return result;
        }

        return CheckAllSituations(next, previous);
    }

    private Expression CheckAllSituations(string next, string? previous)
    {
        if (double.TryParse(next, out var res))
        {
            _position++;
            return System.Linq.Expressions.Expression.Constant(res);
        }

        var message = (previous,next) switch
        {
            (not null, not null) when (previous, _operations.Contains(next)) == ("", true) => StartingWithOperation,
            (not null, not null) when (_operations.Contains(previous), next) == (true, "") => EndingWithOperation,
            (not null, not null) when (previous, _operations.Contains(next)) == ("(", true) => 
                InvalidOperatorAfterParenthesisMessage(next),
            (not null, not null) when (_operations.Contains(previous), next) == (true, ")") => 
                OperationBeforeParenthesisMessage(previous),
            (not null, not null) when (_operations.Contains(previous), _operations.Contains(next)) == (true, true)
                => TwoOperationInRowMessage(previous,next),
            (not null, not null) when (previous, Numbers.IsMatch(next)) == (previous, true) => 
                NotNumberMessage(next),
            _ => UnknownCharacterMessage(next!.ToCharArray()[0]),
        };
        
        throw new ArgumentException(message);
    }
}