using AdventOfCode.Day7;
using Xunit;

namespace AdventOfCodeTests.Day7;

public class Day7Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = FileHelper.ReadFromFile("Day7", "Sample.txt");
        Assert.Equal(95437, Day7Puzzle.GetSumOfSizeOfDirectoriesGreaterThan(input, 100000));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var input = FileHelper.ReadFromFile("Day7", "RealData.txt");
        Assert.Equal(1583951, Day7Puzzle.GetSumOfSizeOfDirectoriesGreaterThan(input, 100000));
    }
    
    [Fact]
    public void Part2_WorksForSampleData()
    {
        var input = FileHelper.ReadFromFile("Day7", "Sample.txt");
        Assert.Equal(24933642, Day7Puzzle.GetSizeOfSmallestDirectoryToDelete(input, 30_000_000, 70_000_000));
    }
    
    [Fact]
    public void Part2_WorksForRealData()
    {
        var input = FileHelper.ReadFromFile("Day7", "RealData.txt");
        Assert.Equal(214171, Day7Puzzle.GetSizeOfSmallestDirectoryToDelete(input, 30_000_000, 70_000_000));
    }
}