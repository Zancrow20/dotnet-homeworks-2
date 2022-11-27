using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hw10.ErrorMessages;
using Hw10.Services.CalculatorVisitor;
using Hw10.Services.ExpressionConverter;
using Xunit;

namespace Homework10.Tests;

public class CalculatorVisitorTests
{
    private readonly CalcVisitorImpl _calculatorVisitor;
    private readonly ExpressionConverter _converter;

    private readonly Dictionary<BinaryExpression, string> _unsupportedOperations = new()
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
        _converter = new ExpressionConverter();
        _calculatorVisitor = new CalcVisitorImpl();
    }

    [Fact]
    public async Task Visit_CalculatorVisitor_Error()
    {
        foreach (var (binaryExpression, message) in _unsupportedOperations)
        {
            try
            {
                _converter.Clear();
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