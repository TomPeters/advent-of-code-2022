using AdventOfCode.Day8;
using Xunit;

namespace AdventOfCodeTests.Day8;

public class Day8Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = FileHelper.ReadFromFile("Day8", "Sample.txt");
        Assert.Equal(0, Day8Puzzle.GetNumberOfVisibileTrees(input));
    }
}