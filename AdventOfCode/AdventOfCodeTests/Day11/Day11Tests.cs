using AdventOfCode.Day11;
using Xunit;

namespace AdventOfCodeTests.Day11;

public class Day11Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        Assert.Equal(13140, Day11Puzzle.GetLevelOfMonkeyBusiness(20));
    }
}