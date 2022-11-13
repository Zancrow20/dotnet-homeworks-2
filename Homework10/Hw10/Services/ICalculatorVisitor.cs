using System.Linq.Expressions;

namespace Hw10.Services;

public interface ICalculatorVisitor
{
    Task<double> CalculatorVisitBinary(Dictionary<Expression, Expression[]> expressionsMap);
}