using Hw11.Dto;
using Hw11.Services.CalculatorVisitor;
using Hw11.Services.RecursiveParser;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        var calculatorVisitor = new CalcVisitorImpl();
        var expressionConverter = new ExpressionConverter.ExpressionConverter();
        var parser = new RecursiveDescentParser(expression);
        
        var expressionTree = parser.Parse();
        var expressionMap = expressionConverter.GetExpressionsMap(expressionTree);
        var result = await calculatorVisitor.CalculatorVisit(expressionMap);
        return new CalculationMathExpressionResultDto(result).Result;
    }
}