using System.Collections.Concurrent;
using Hw13CacheCalculator.Dto;
using Microsoft.Extensions.Caching.Memory;


namespace Hw13CacheCalculator.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
    private readonly IMathCalculatorService _simpleCalculator;
    private readonly ICacheManagerService? _cacheManager;

	public MathCachedCalculatorService(IMathCalculatorService simpleCalculator)
	{
		_cacheManager = new CacheManagerServiceService();
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var result = await _cacheManager
			.GetOrAdd(expression,async () => await _simpleCalculator.CalculateMathExpressionAsync(expression));
		return result;
	}
}

public class CacheManagerServiceService : ICacheManagerService
{
	private MemoryCache _cache = new (new MemoryCacheOptions());
	private ConcurrentDictionary<string, SemaphoreSlim> _locks = new ();
 
	public async Task<CalculationMathExpressionResultDto> GetOrAdd(string? expression, 
		Func<Task<CalculationMathExpressionResultDto>> createItem)
	{
		if (string.IsNullOrEmpty(expression))
			return new CalculationMathExpressionResultDto(ErrorMessages.MathErrorMessager.EmptyString);
		if (!_cache.TryGetValue(expression, out CalculationMathExpressionResultDto cacheEntry))
		{
			var myLock = _locks.GetOrAdd(expression, new SemaphoreSlim(1, 1));
 
			await myLock.WaitAsync();
			try
			{
				if (!_cache.TryGetValue(expression, out cacheEntry))
				{
					cacheEntry = await createItem();
					var cacheEntryOptions =
						new MemoryCacheEntryOptions()
							.SetSlidingExpiration(TimeSpan.FromMinutes(1))
							.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
					_cache.Set(expression, cacheEntry, cacheEntryOptions);
				}
			}
			finally
			{
				myLock.Release();
			}
		}
		return cacheEntry;
	}    
}