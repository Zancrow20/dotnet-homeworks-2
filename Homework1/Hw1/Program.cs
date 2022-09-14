


using Hw1;
public class Program
{
    public static int Main(string[] args)
    {
        try
        {
            Parser.ParseCalcArguments(args, out double arg1, out CalculatorOperation operation, out double arg2);
            var res = Calculator.Calculate(arg1,operation,arg2);
            Console.WriteLine(res);
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception : {ex.Message}");
            return -1;
        }
    }
}