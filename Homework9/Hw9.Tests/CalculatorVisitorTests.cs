using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http.Json;
using Hw9.Dto;
using Hw9.ErrorMessages;
using Hw9.CalculatorVisitor;
using Hw9.ExpressionsConverter;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Hw9.Tests;

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