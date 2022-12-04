using System.Linq.Expressions;

namespace Hw13CacheCalculator.Services.ExpressionConverter;

public class ExpressionConverter
{
    private readonly Dictionary<Expression, Expression[]> ExpressionsMap = new();

    public Dictionary<Expression, Expression[]> GetExpressionsMap(Expression expression)
    {
        Visit(expression);
        return ExpressionsMap;
    }

    private void Visit(Expression expression)
    {
        if (!ExpressionsMap.ContainsKey(expression))
            switch (expression)
            {
                case BinaryExpression binaryExpression:
                    VisitBinary(binaryExpression);
                    break;
                case ConstantExpression constantExpression:
                    VisitConstant(constantExpression);
                    break;
            }
    }

    private void VisitBinary(BinaryExpression binaryExpression)
    {
        ExpressionsMap.Add(binaryExpression, new[] {binaryExpression.Left, binaryExpression.Right});
        Visit(binaryExpression.Left);
        Visit(binaryExpression.Right);
    }

    private void VisitConstant(ConstantExpression constantExpression) =>
        ExpressionsMap.Add(constantExpression, Array.Empty<Expression>());
}
