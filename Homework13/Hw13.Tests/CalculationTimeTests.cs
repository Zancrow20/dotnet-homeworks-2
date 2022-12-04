using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Hw13CacheCalculator;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Hw13.Tests;

public class CalculationTimeTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CalculationTimeTests(WebApplicationFactory<Program> fixture)
    {
        _client = fixture.CreateClient();
    }
    
    [Theory]
    [InlineData("2 + 3 + 4 + 6", 2990, 4500)]
    [InlineData("(2 * 3 + 3 * 3) * (5 / 5 + 6 / 6)", 2990, 4500)]
    [InlineData("(2 + 3) / 12 * 7 + 8 * 9", 3990, 5500)]
    private async Task CalculatorController_ParallelTest(string expression, long minExpectedTime, long maxExpectedTime)
    {
        var executionTime = await GetRequestExecutionTime(expression);
        
        Assert.True(executionTime >= minExpectedTime, 
            UserMessagerForTest.WaitingTimeIsLess(minExpectedTime, executionTime));
        Assert.True(executionTime <= maxExpectedTime, 
            UserMessagerForTest.WaitingTimeIsMore(maxExpectedTime, executionTime));
    }
    
    [Theory]
    [InlineData("1 + 1 + 1 + 1")]
    [InlineData("2 * (3 + 2) / 2")]
    [InlineData("2 * 3 / 1 * 5 * 6")]
    private async Task Calculate_CacheTest(string expression)
    {
        await GetRequestExecutionTime(expression);
        var secondCalculationTime = await GetRequestExecutionTime(expression);
        Assert.True(secondCalculationTime <= 1500);
    }
    
    private async Task<long> GetRequestExecutionTime(string expression)
    {
        var watch = Stopwatch.StartNew();
        var response = await _client.PostCalculateExpressionAsync(expression);
        watch.Stop();
        response.EnsureSuccessStatusCode();
        return watch.ElapsedMilliseconds;
    }
}