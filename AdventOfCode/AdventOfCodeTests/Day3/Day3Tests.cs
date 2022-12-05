using AdventOfCode;
using Xunit;

namespace AdventOfCodeTests.Day3;

public class Day3Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = FileHelper.ReadFromFile("Day3", "Sample.txt");
        Assert.Equal(157, Day3Puzzle.GetResult(input));
    }
}