using AdventOfCode.Day14;
using Xunit;

namespace AdventOfCodeTests.Day14;

public class Day14Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day13", "Sample.txt"));
        Assert.Equal(24, Day14Puzzle.GetUnitsOfSandThatFlowBeforeTheRestFlowIntoTheAbyss(input));
    }

    private string ParseInput(string input)
    {
        return input;
    }
}