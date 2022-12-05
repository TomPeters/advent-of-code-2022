using AdventOfCode;
using Xunit;

namespace AdventOfCodeTests.StringExtensions;

public class StringExtensionsTests
{
    [Fact]
    public void Transpose()
    {
        var input = @"ABCD
EFGH
IJKL
MNOP";

        var expected = @"AEIM
BFJN
CGKO
DHLP";
        Assert.Equal(expected, input.Transpose());
    }
}
