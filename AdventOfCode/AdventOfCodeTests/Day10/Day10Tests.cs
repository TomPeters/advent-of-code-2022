using AdventOfCode.Day10;
using Xunit;

namespace AdventOfCodeTests.Day10;

public class Day10Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day9", "Sample.txt"));
        Assert.Equal(13140, Day10Puzzle.GetSumOfSignalStrengths(input));
    }

    static string ParseInput(string input) => input;
}