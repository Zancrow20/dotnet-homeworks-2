using Hw10.Dto;
using Hw10.Services.CalculatorVisitor;
using Hw10.Services.RecursiveParser;

namespace Hw10.Services.MathCalculator;

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