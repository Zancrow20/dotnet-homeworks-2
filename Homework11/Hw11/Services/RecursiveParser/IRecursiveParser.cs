using System.Linq.Expressions;

namespace Hw11.Services;

public interface IRecursiveParser
{
    Expression Parse();
}