using AdventOfCode.Day1;
using AdventOfCode.Day2;
using Xunit;

namespace AdventOfCodeTests.Day2;

public class Day2Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = FileHelper.ReadFromFile("Day2", "Sample.txt");
        Assert.Equal(15, Day2Puzzle.GetTotalScore(input));
    }
}