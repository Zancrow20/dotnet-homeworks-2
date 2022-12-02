using System.Linq.Expressions;

namespace Hw13CacheCalculator.Services;

public interface IRecursiveParser
{
    Expression Parse();
}