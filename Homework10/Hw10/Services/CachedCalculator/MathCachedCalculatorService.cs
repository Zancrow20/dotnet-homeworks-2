using Hw10.DbModels;
using Hw10.Dto;
using Hw10.Services.MathCalculator;
using Microsoft.EntityFrameworkCore;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		var cache = await _dbContext.SolvingExpressions
			.FirstOrDefaultAsync(cacheFromDb => cacheFromDb.Expression == expression);
		if (cache is null)
			return await ExecuteAndSave(expression);
		await Task.Delay(1000);
		return new CalculationMathExpressionResultDto(cache.Result);
	}

	private async Task<CalculationMathExpressionResultDto> ExecuteAndSave(string expression)
	{
		var resultDto = await _simpleCalculator.CalculateMathExpressionAsync(expression);
		if (resultDto.IsSuccess)
		{
			var solvingExpression = new SolvingExpression { Expression = expression, Result = resultDto.Result};
			await _dbContext.SolvingExpressions.AddAsync(solvingExpression);
			await _dbContext.SaveChangesAsync();
		}
		return resultDto;
	}
}