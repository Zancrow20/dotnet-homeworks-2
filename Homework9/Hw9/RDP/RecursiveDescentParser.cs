using System.Linq.Expressions;
using System.Text.RegularExpressions;
using static Hw9.ErrorMessages.MathErrorMessager;
namespace Hw9.RDP;

public class RecursiveDescentParser
{
    private readonly string[] _operations = {"+", "-", "*", "/"};

    private readonly string[]? _tokens;
    private int _position;
    private readonly bool IsGoodExpression;

    private static readonly Regex InputSplit = new ("(?<=[−+*/\\(\\)])|(?=[−+*/\\(\\)])");
    private static readonly Regex Numbers = new("[0-9]+");
    
    public RecursiveDescentParser(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new NullReferenceException(EmptyString);
        IsGoodExpression = true;
            _tokens = InputSplit.Split(expression)
                .SelectMany(str => str.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToArray();
    }

    public Expression Parse() 
    {
        var result = Expression();
        if (_position == _tokens?.Length || !IsGoodExpression) return result;
        var message = _tokens?[_position] == ")" ? IncorrectBracketsNumber : 
            UnknownCharacterMessage(_tokens![_position].ToCharArray()[0]);
        throw new ArgumentException(message);
    }
    
    private Expression Expression() 
    {
        var firstExpression= Term();
        while (_position < _tokens?.Length)
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
        while (_position < _tokens?.Length) 
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
        var next = _position < _tokens?.Length ? _tokens[_position] : string.Empty;
        var previous = _position - 1 >= 0 ? _tokens?[_position - 1] : string.Empty;
        if (next.Equals("(")) {
            _position++;
            var result = Expression();
            if (_position >= _tokens?.Length)//Если войдёт дважды в рекурсию и не будет закрывающих скобок, то ошибка
                throw new ArgumentException(IncorrectBracketsNumber);
            
            _position++;
            return result;
        }
        
        return CheckAllSituations(next, previous);
    }

    private Expression  CheckAllSituations(string next, string? previous)
    {
        if (double.TryParse(next, out var res))
        {
            _position++;
            return System.Linq.Expressions.Expression.Constant(res);
        }
        string message;
        if (_position == 0 && _operations.Contains(next))
            message = StartingWithOperation;
        else if (next == ")" && _operations.Contains(previous))
            message = OperationBeforeParenthesisMessage(previous ?? string.Empty);
        else if (previous == "(" && _operations.Contains(next))
            message =InvalidOperatorAfterParenthesisMessage(next);
        else if (_operations.Contains(previous) && _operations.Contains(next))
            message = TwoOperationInRowMessage(previous ?? string.Empty, next);
        else if (Numbers.IsMatch(next))
            message = NotNumberMessage(next);
        else if (_operations.Contains(previous) && next == string.Empty)
            message = EndingWithOperation;
        else
            message = UnknownCharacterMessage(next.ToCharArray()[0]);
        throw new ArgumentException(message);
    }
}