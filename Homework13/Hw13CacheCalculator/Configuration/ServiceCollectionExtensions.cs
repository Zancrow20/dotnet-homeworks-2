using Hw13CacheCalculator.Services;
using Hw13CacheCalculator.Services.CachedCalculator;
using Hw13CacheCalculator.Services.MathCalculator;
using Microsoft.Extensions.Caching.Memory;

namespace Hw13CacheCalculator.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        services.AddSingleton<IMemoryCache,MemoryCache>();
        return services.AddScoped<IMathCalculatorService, MathCachedCalculatorService>();
    }
}