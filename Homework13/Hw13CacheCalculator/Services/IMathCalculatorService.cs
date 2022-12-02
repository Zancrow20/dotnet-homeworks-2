using Hw13CacheCalculator.Dto;

namespace Hw13CacheCalculator.Services;

public interface IMathCalculatorService
{
    public Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression);
}