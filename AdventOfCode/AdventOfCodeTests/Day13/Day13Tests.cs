using AdventOfCode.Day13;
using Xunit;

namespace AdventOfCodeTests.Day13;

public class Day13Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day13", "Sample.txt"));
        Assert.Equal(13, Day13Puzzle.GetSumOfIndicesOfCorrectlyOrderedPairs(input));
    }

    private string ParseInput(string input)
    {
        return input;
    }
}