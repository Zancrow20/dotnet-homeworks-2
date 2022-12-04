using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hw13CacheCalculator.ErrorMessages;
using Hw13CacheCalculator.Services.CalculatorVisitor;
using Hw13CacheCalculator.Services.ExpressionConverter;
using Xunit;

namespace Hw13.Tests;

public class CalculatorVisitorTests
{
    private CalcVisitorImpl _calculatorVisitor;
    private ExpressionConverter _converter;

    private Dictionary<BinaryExpression, string> unsupportedOperations = new()
    {
        {Expression.AddChecked(Expression.Constant((double)5), Expression.Constant((double)5)),
            "calculator doesn't support this operation"},
        {Expression.MultiplyChecked(Expression.Constant((double)5), Expression.Constant((double)5)),
            "calculator doesn't support this operation"},
        {Expression.SubtractChecked(Expression.Constant((double)11), Expression.Constant((double)5)),
            "calculator doesn't support this operation"},
        {Expression.Divide(Expression.Constant((double)10), Expression.Constant((double)0)), 
            MathErrorMessager.DivisionByZero}
    };

    public CalculatorVisitorTests()
    {
        _calculatorVisitor = new CalcVisitorImpl();
    }

    [Fact]
    public async Task Visit_CalculatorVisitor_Error()
    {
        foreach (var (binaryExpression, message) in unsupportedOperations)
        {
            try
            {
                _converter = new ExpressionConverter();
                var expressionsMap = _converter.GetExpressionsMap(binaryExpression);
                var result = await _calculatorVisitor.CalculatorVisitBinary(expressionsMap);
            }
            catch (Exception ex)
            {
                Assert.Equal(message,ex.Message);
            }
        }
    }
}