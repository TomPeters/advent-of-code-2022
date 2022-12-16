using AdventOfCode.Day16;
using Xunit;

namespace AdventOfCodeTests.Day16;

public class Day16Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day16", "Sample.txt"));
        Assert.Equal(1651, Day16Puzzle.GetTheMostPressureThatCanBeReleased(input));
    }

    private string ParseInput(string input)
    {
        return input;
    }
}