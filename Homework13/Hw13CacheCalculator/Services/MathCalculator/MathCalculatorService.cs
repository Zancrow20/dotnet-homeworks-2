using Hw13CacheCalculator.Dto;
using Hw13CacheCalculator.Services.CalculatorVisitor;
using Hw13CacheCalculator.Services.RecursiveParser;

namespace Hw13CacheCalculator.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var calculatorVisitor = new CalcVisitorImpl();
        var expressionConverter = new ExpressionConverter.ExpressionConverter();
        try
        {
            var parser = new RecursiveDescentParser(expression);
            var expressionTree = parser.Parse();
            var expressionMap = expressionConverter.GetExpressionsMap(expressionTree);
            var result = await calculatorVisitor.CalculatorVisitBinary(expressionMap);
            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception e)
        {
            return new CalculationMathExpressionResultDto(e.Message);
        }
    }
}