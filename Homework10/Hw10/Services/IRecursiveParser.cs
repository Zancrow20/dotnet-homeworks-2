using System.Linq.Expressions;

namespace Hw10.Services;

public interface IRecursiveParser
{
    Expression Parse();
}