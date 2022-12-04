using System.Linq.Expressions;

namespace Hw13CacheCalculator.Services;

public interface ICalculatorVisitor
{
    Task<double> CalculatorVisitBinary(Dictionary<Expression, Expression[]> expressionsMap);
}