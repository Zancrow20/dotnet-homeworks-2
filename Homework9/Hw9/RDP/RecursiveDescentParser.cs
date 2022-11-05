using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Hw9.ReversePolishNotation;
using static Hw9.ErrorMessages.MathErrorMessager;
namespace Hw9.RDP;

public class RecursiveDescentParser
{
    private readonly string[] _operations = {"+", "-", "*", "/"};

    private readonly string[]? _tokens;
    private int _position;
    public Status StatusOfExpression;

    private static readonly Regex InputSplit = new ("(?<=[−+*/\\(\\)])|(?=[−+*/\\(\\)])");
    private static readonly Regex Numbers = new("[0-9]+");
    
    public RecursiveDescentParser(string? expression)
    {
        if (string.IsNullOrEmpty(expression))
            StatusOfExpression = new Status(EmptyString);
        else
        {
            StatusOfExpression = new Status();
            _tokens = InputSplit.Split(expression)
                .SelectMany(str => str.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToArray();
        }
    }

    public Expression Parse() 
    {
        var result = !StatusOfExpression.IsGoodExpression ? 
            System.Linq.Expressions.Expression.Constant(-1) :
            Expression();
        if (_position == _tokens?.Length || !StatusOfExpression.IsGoodExpression) return result;
        if (_tokens?[_position] == ")")
            StatusOfExpression = new Status(IncorrectBracketsNumber);
        else if (!_operations.Contains(_tokens?[_position]))
            StatusOfExpression = new Status(UnknownCharacterMessage(_tokens![_position].ToCharArray()[0]));
        result = System.Linq.Expressions.Expression.Constant(-1);
        return result;
    }
    
    private Expression Expression() 
    {
        var firstExpression= Term();
        if (!StatusOfExpression.IsGoodExpression) return firstExpression;
        while (_position < _tokens?.Length)
        {
            var token = _tokens[_position];
            if (!token.Equals("+") && !token.Equals("-")) break;
            _position++;
            var secondExpression = Term();
            if (StatusOfExpression.IsGoodExpression)
                firstExpression = token.Equals("+")
                    ? System.Linq.Expressions.Expression.Add(firstExpression, secondExpression)
                    : System.Linq.Expressions.Expression.Subtract(firstExpression, secondExpression);
            else
                return System.Linq.Expressions.Expression.Constant(-1);
        }

        return firstExpression;
    }

    
    private Expression Term() 
    {
        var firstExpression = Factor();
        if (!StatusOfExpression.IsGoodExpression) return firstExpression;
        while (_position < _tokens?.Length) 
        {
            var token = _tokens[_position];
            if (!token.Equals("*") && !token.Equals("/")) break;
            _position++;
            var secondExpression = Factor();
            if (StatusOfExpression.IsGoodExpression)
                firstExpression = token.Equals("*")
                    ? System.Linq.Expressions.Expression.Multiply(firstExpression, secondExpression)
                    : System.Linq.Expressions.Expression.Divide(firstExpression, secondExpression);
            else
                return System.Linq.Expressions.Expression.Constant(-1);
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
            if (!StatusOfExpression.IsGoodExpression)
                return result;
            if (_position >= _tokens?.Length)
            {
                StatusOfExpression = new Status(IncorrectBracketsNumber);
                return System.Linq.Expressions.Expression.Constant(-1);
            }
            
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
        
        if (_position == 0 && _operations.Contains(next))
            StatusOfExpression = new Status(StartingWithOperation);
        else if (next == ")" && _operations.Contains(previous))
            StatusOfExpression = new Status(OperationBeforeParenthesisMessage(previous ?? string.Empty));
        else if (previous == "(" && _operations.Contains(next))
            StatusOfExpression = new Status(InvalidOperatorAfterParenthesisMessage(next));
        else if (_operations.Contains(previous) && _operations.Contains(next))
            StatusOfExpression = new Status(TwoOperationInRowMessage(previous ?? string.Empty, next));
        else if (Numbers.IsMatch(next))
            StatusOfExpression = new Status(NotNumberMessage(next));
        else if(_operations.Contains(previous) && next == string.Empty)
            StatusOfExpression = new Status(EndingWithOperation);
        else
            StatusOfExpression = new Status(UnknownCharacterMessage(next.ToCharArray()[0]));
        return System.Linq.Expressions.Expression.Constant(-1);
    }
}