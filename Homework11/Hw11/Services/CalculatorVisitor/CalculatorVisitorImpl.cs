using System.Linq.Expressions;
using Hw11.Exceptions;
using static Hw11.ErrorMessages.MathErrorMessager;

namespace Hw11.Services.CalculatorVisitor;

public class CalcVisitorImpl : ICalculatorVisitor
{
    private Dictionary<Expression,Lazy<Task<double>>> _lazy = new ();
    public async Task<double> CalculatorVisit( 
        Dictionary<Expression, Expression[]> expressionsMap)
    {
        var root = expressionsMap.Keys.First();
        foreach (var (expression, doBefore) in expressionsMap)
            _lazy[expression] = new Lazy<Task<double>>(async () =>
            {
                await Task.WhenAll(doBefore.Select(b => _lazy[b].Value));
                await Task.Yield();
                
                return await CalculateExpressions(expression as dynamic);
            });
        var value = await _lazy[root].Value;
        _lazy.Clear();
        return value;
    }

    private static double Add(double leftNode, double rightNode)
        => leftNode + rightNode;

    private static double Subtract(double leftNode, double rightNode)
        => leftNode - rightNode;

    private static double Multiply(double leftNode, double rightNode)
        => leftNode * rightNode;

    private static double Divide(double leftNode, double rightNode)
        => rightNode != 0 ? leftNode / rightNode : 
            throw new DivideByZeroException(DivisionByZero);

    private async Task<double> CalculateExpressions(Expression expression)
        {
            if (expression is ConstantExpression constantExpression) 
                return await Task.FromResult((double) constantExpression.Value!);
            
            await Task.Delay(1000);
            var binaryExpression = expression as BinaryExpression;
            var leftNode = await _lazy[binaryExpression.Left].Value;
            var rightNode = await _lazy[binaryExpression.Right].Value;
            
            return expression.NodeType switch 
            {
                ExpressionType.Add => Add(leftNode, rightNode),
                ExpressionType.Subtract => Subtract(leftNode, rightNode),
                ExpressionType.Multiply => Multiply(leftNode, rightNode),
                ExpressionType.Divide => Divide(leftNode,rightNode),
                _ => throw new NotImplementedException("calculator doesn't support this operation")
            };
        }
}