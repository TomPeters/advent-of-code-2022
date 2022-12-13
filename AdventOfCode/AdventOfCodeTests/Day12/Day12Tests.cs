using AdventOfCode.Day12;
using Xunit;

namespace AdventOfCodeTests.Day12;

public class Day12Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day12", "Sample.txt"));
        Assert.Equal(31, Day12Puzzle.GetLengthOfShortestPath(input));
    }

    static string ParseInput(string input)
    {
        return input;
    }
}