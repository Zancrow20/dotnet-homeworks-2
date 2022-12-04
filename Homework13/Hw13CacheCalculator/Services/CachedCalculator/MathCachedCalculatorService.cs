using Hw13CacheCalculator.Dto;
using Microsoft.Extensions.Caching.Memory;
using static Hw13CacheCalculator.ErrorMessages.MathErrorMessager;


namespace Hw13CacheCalculator.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
    private readonly IMathCalculatorService _simpleCalculator;
    private readonly IMemoryCache _cache;

	public MathCachedCalculatorService(IMemoryCache cache, IMathCalculatorService simpleCalculator)
	{
		_cache = cache;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		if (string.IsNullOrWhiteSpace(expression))
			return new CalculationMathExpressionResultDto(EmptyString);
		var cache = await _cache.GetOrCreateAsync(expression, entry =>
		{
			entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
			return _simpleCalculator.CalculateMathExpressionAsync(expression);
		});
		await Task.Delay(1000);
		return cache;
	}
}