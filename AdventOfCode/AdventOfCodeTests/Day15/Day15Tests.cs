using AdventOfCode.Day15;
using Xunit;

namespace AdventOfCodeTests.Day15;

public class Day15Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day15", "Sample.txt"));
        Assert.Equal(26, Day15Puzzle.GetResult(input));
    }

    private string ParseInput(string input)
    {
        return input;
    }
}