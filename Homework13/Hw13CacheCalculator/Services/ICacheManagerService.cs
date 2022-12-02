using Hw13CacheCalculator.Dto;

namespace Hw13CacheCalculator.Services;

public interface ICacheManagerService
{
    Task<CalculationMathExpressionResultDto> GetOrAdd(string? expression,
        Func<Task<CalculationMathExpressionResultDto>> createItem);
}