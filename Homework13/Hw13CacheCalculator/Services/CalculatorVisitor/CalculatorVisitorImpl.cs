using System.Linq.Expressions;
using static Hw13CacheCalculator.ErrorMessages.MathErrorMessager;

namespace Hw13CacheCalculator.Services.CalculatorVisitor;

public class CalcVisitorImpl : ICalculatorVisitor
{
    public async Task<double> CalculatorVisitBinary( 
        Dictionary<Expression, Expression[]> expressionsMap)
    {
        var root = expressionsMap.Keys.First();
        var lazy = new Dictionary<Expression, Lazy<Task<double>>>();
        foreach (var (expression, doBefore) in expressionsMap)
            lazy[expression] = new Lazy<Task<double>>(async () =>
            {
                await Task.WhenAll(doBefore.Select(b => lazy[b].Value));
                await Task.Yield();

                if(expression is BinaryExpression binaryExpression)
                    return await CalculateExpressions(expression, await lazy[binaryExpression.Left].Value,
                    await lazy[binaryExpression.Right].Value);
                return await CalculateExpressions(expression, 0, 0);
            });
        return await lazy[root].Value;
    }

    private static double Add(double leftNode, double rightNode)
        => leftNode + rightNode;

    private static double Subtract(double leftNode, double rightNode)
        => leftNode - rightNode;

    private static double Multiply(double leftNode, double rightNode)
        => leftNode * rightNode;

    private static double Divide(double leftNode, double rightNode)
        => rightNode != 0 ? leftNode / rightNode : throw new DivideByZeroException(DivisionByZero);

    private static async Task<double> CalculateExpressions(Expression expression, double leftNode, double rightNode)
        {
            if (expression is ConstantExpression constantExpression) 
                return await Task.FromResult((double) constantExpression.Value!);
    
            await Task.Delay(1000);

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