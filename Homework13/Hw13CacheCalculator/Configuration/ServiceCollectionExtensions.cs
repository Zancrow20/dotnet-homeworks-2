using Hw13CacheCalculator.Services;
using Hw13CacheCalculator.Services.CachedCalculator;
using Hw13CacheCalculator.Services.MathCalculator;
using Microsoft.Extensions.Caching.Memory;

namespace Hw13CacheCalculator.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services.AddTransient<MathCalculatorService>();
    }
    
    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        return services.AddScoped<IMathCalculatorService>(s =>
            new MathCachedCalculatorService(s.GetRequiredService<IMemoryCache>(),
                s.GetRequiredService<MathCalculatorService>()));
    }
}