using AdventOfCode;
using Xunit;

namespace AdventOfCodeTests.Day3;

public class ContentTests
{
    [Fact]
    public void LowerCaseA_HasPriority_1()
    {
        Assert.Equal(1, new Content('a').GetPriority());
    }
    
    [Fact]
    public void LowerCaseZ_HasPriority_26()
    {
        Assert.Equal(26, new Content('z').GetPriority());
    }
    
    [Fact]
    public void UpperCaseA_HasPriority_27()
    {
        Assert.Equal(27, new Content('A').GetPriority());
    }
    
    [Fact]
    public void UpperCaseZ_HasPriority_52()
    {
        Assert.Equal(52, new Content('Z').GetPriority());
    }
}