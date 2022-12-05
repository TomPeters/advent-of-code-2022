using AdventOfCode.Day5;
using Xunit;

namespace AdventOfCodeTests.Day5;

public class Day5Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = FileHelper.ReadFromFile("Day5", "Sample.txt");
        Assert.Equal("CMZ", Day5Puzzle.GetTopCratesAsString(input));
    }
}