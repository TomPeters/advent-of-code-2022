using AdventOfCode.Day7;
using Xunit;

namespace AdventOfCodeTests.Day7;

public class Day7Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day7", "Sample.txt"));
        Assert.Equal(95437, Day7Puzzle.GetSumOfSizeOfDirectoriesGreaterThan(input, 100000));
    }

    static string ParseInput(string input)
    {
        return input;
    }
}