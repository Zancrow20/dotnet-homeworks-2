using Hw9.Dto;
using System.Linq.Expressions;
using Hw9.RDP;
using static Hw9.ErrorMessages.MathErrorMessager;
using Hw9.CalculatorVisitor;
using Hw9.ExpressionsConverter;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var calculatorVisitor = new CalcVisitorImpl();
        var expressionConverter = new ExpressionConverter();
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