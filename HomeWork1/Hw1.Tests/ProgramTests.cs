using Hw1;
using Xunit;

namespace Hw1Tests;

public class ProgramTests
{
    [Theory]
    [InlineData("1", "+", "2")]
    [InlineData("1", "/", "0")]
    [InlineData("0", "/", "0")]
    [InlineData("2","/", "1")]
    [InlineData("2", "-", "2")]
    public void TestCorrectProgram(params string[] input)
    {
        var value = Program.Main(input);
        Assert.Equal(0,value);
    }

    [Theory]
    [InlineData("1", "+")]
    [InlineData("1", ".", "3")]
    [InlineData("1", "2", "3")]
    [InlineData("1", "2", "3")]
    [InlineData("f", "+", "3")]
    public void TestProgramWrongInput(params string[] input)
    {
        var value = Program.Main(input);
        Assert.Equal(-1, value);
    }
}