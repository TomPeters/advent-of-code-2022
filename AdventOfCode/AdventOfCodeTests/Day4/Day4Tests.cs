using AdventOfCode.Day4;
using Xunit;

namespace AdventOfCodeTests.Day4;

public class Day4Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = FileHelper.ReadFromFile("Day4", "Sample.txt");
        Assert.Equal(2, Day4Puzzle.GetNumberOfAssignmentPairsWhereOneContainsTheOther(input));
    }
}