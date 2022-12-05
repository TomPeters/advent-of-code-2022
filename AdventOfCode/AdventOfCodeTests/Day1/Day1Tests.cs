using AdventOfCode.Day1;
using Xunit;

namespace AdventOfCodeTests.Day1;

public class Day1Tests
{
    [Fact]
    public void WorksForSampleData()
    {
        var sampleData = FileHelper.ReadFromFile("Day1", "Sample.txt");
        Assert.Equal(24000, Day1Puzzle.GetResult(sampleData));
    }
}