using Hw9.Dto;
using System.Linq.Expressions;
using Hw9.RDP;
using static Hw9.ErrorMessages.MathErrorMessager;
using Hw9.CalculatorVisitor;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var parser = new RecursiveDescentParser(expression);
        var (expressionTree, isGoodExpression) = parser.Parse();
        if (!isGoodExpression)
            return new CalculationMathExpressionResultDto(parser.StatusOfExpression.ErrorMessage);
        var result = (double)((ConstantExpression)new CalcVisitorImpl().Visit(expressionTree)).Value!;
        return double.IsNaN(result) ? new CalculationMathExpressionResultDto(DivisionByZero) : 
            new CalculationMathExpressionResultDto(result);
    }
}