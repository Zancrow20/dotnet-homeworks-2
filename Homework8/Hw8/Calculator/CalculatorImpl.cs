using System.Diagnostics;

namespace Hw8.Calculator;

public class CalculatorImpl : ICalculator
{
    public double Plus(double val1, double val2) => val1 + val2;

    public double Minus(double val1, double val2) => val1 - val2;

    public double Multiply(double val1, double val2) => val1 * val2;

    public double Divide(double firstValue, double secondValue) =>
        secondValue == 0 ? throw new InvalidOperationException(Messages.DivisionByZeroMessage) : firstValue / secondValue;

    public double Calculate(double firstValue, Operation operation, double secondValue)
    {
        return operation switch
        {
            Operation.Plus => Plus(firstValue, secondValue),
            Operation.Minus => Minus(firstValue, secondValue),
            Operation.Multiply => Multiply(firstValue, secondValue),
            Operation.Divide => Divide(firstValue, secondValue),
            _ => throw new InvalidOperationException(Messages.InvalidOperationMessage)
        };
    }
}