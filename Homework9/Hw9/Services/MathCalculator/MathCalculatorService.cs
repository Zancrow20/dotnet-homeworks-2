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
        var calculatorVisitor = new CalcVisitorImpl();
        
        try
        {
            var parser = new RecursiveDescentParser(expression);
            var expressionTree = parser.Parse();
            var result = (double)((ConstantExpression)calculatorVisitor.Visit(expressionTree)).Value!;
            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception e)
        {
            return new CalculationMathExpressionResultDto(e.Message);
        }
    }
}