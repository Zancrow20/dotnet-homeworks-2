using System.Linq.Expressions;

namespace Hw11.Services;

public interface ICalculatorVisitor
{
    Task<double> CalculatorVisit(Dictionary<Expression, Expression[]> expressionsMap);
}