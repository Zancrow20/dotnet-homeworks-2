using System.Linq.Expressions;

namespace Hw11.Services.ExpressionConverter;

public interface IExpressionConverter
{
    Dictionary<Expression, Expression[]> GetExpressionsMap(Expression expression);
}