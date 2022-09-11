

using Hw1;

Parser.ParseCalcArguments(args, out double arg1, out CalculatorOperation operation, out double arg2);
Console.WriteLine(Calculator.Calculate(arg1,operation,arg2));