using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;
using static Hw8.Calculator.Messages;
using Hw8.Parser;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    private IParser _parser;
    public CalculatorController(IParser parser)
    {
        _parser = parser;
    }
    
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        [FromQuery] string? val1,
        [FromQuery] string? operation,
        [FromQuery] string? val2)
    {
        var message = _parser.TryParseValues(val1, operation, val2, out var res);
        return message switch
        {
            OK => Ok(calculator.Calculate(res.firstValue, res.operation, res.secondValue)),
            InvalidNumberMessage => BadRequest(InvalidNumberMessage),
            InvalidOperationMessage => BadRequest(InvalidOperationMessage),
            DivisionByZeroMessage => BadRequest(DivisionByZeroMessage),
            _ => BadRequest("Введите достаточное количество аргументов")
        };
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}