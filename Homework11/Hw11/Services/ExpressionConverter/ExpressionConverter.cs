using System.Linq.Expressions;

namespace Hw11.Services.ExpressionConverter;

public class ExpressionConverter : IExpressionConverter
{
    private readonly Dictionary<Expression, Expression[]> ExpressionsMap = new();

    public Dictionary<Expression, Expression[]> GetExpressionsMap(Expression expression)
    {
        Visit(expression as dynamic);
        return ExpressionsMap;
    }
    

    private void Visit(BinaryExpression binaryExpression)
    {
        ExpressionsMap.Add(binaryExpression, new[] {binaryExpression.Left, binaryExpression.Right});
        Visit(binaryExpression.Left as dynamic);
        Visit(binaryExpression.Right as dynamic);
    }

    private void Visit(ConstantExpression constantExpression) =>
        ExpressionsMap.Add(constantExpression, Array.Empty<Expression>());

    public void Clear()
        => ExpressionsMap.Clear();
}
