using AdventOfCode.Day9;
using Xunit;

namespace AdventOfCodeTests.Day9;

public class Day9Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day9", "Sample.txt"));
        Assert.Equal(13, Day9Puzzle.GetNumberOfPositionsVisitedByRopeTail(input));
    }

    static string ParseInput(string input) => input;
}