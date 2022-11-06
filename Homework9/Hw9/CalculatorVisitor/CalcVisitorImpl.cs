using System.Linq.Expressions;
using static Hw9.ErrorMessages.MathErrorMessager;

namespace Hw9.CalculatorVisitor;

public class CalcVisitorImpl : ExpressionVisitor
{
    protected override Expression VisitBinary(BinaryExpression node)
    {
        var task = CalculateExpressionsAsync(node);
        var result = task.Result;
        if (double.IsNaN(result))
            throw new DivideByZeroException(DivisionByZero);
        return Expression.Constant(result);
    }

    private static double Add(double leftNode, double rightNode)
        => leftNode + rightNode;

    private static double Subtract(double leftNode, double rightNode)
        => leftNode - rightNode;

    private static double Multiply(double leftNode, double rightNode)
        => leftNode * rightNode;

    private static double Divide(double leftNode, double rightNode)
        => rightNode != 0 ? leftNode / rightNode : Double.NaN;

    private static async Task<double> CalculateExpressionsAsync(Expression expression)
        {
            if (expression is ConstantExpression constantExpression) 
                return await Task.FromResult((double) constantExpression.Value!);
    
            var binaryExpression = (BinaryExpression)expression;
            await Task.Delay(1000);
            var leftNode = CalculateExpressionsAsync(binaryExpression.Left);
            var rightNode = CalculateExpressionsAsync(binaryExpression.Right);
            
            Task.WaitAll(leftNode, rightNode);

            return binaryExpression.NodeType switch 
            {
                ExpressionType.Add => Add(leftNode.Result, rightNode.Result),
                ExpressionType.Subtract => Subtract(leftNode.Result, rightNode.Result),
                ExpressionType.Multiply => Multiply(leftNode.Result, rightNode.Result),
                ExpressionType.Divide => Divide(leftNode.Result,rightNode.Result)
            };
        }
}