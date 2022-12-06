using System.Linq;
using AdventOfCode.Day6;
using Xunit;

namespace AdventOfCodeTests.Day6;

public class Day6Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var dataStreamBuffer = ParseInput(FileHelper.ReadFromFile("Day6", "Sample.txt"));
        Assert.Equal(7, Day6Puzzle.GetNumberOfCharactersProcessedBeforeFirstStartOfPacketMarker(dataStreamBuffer));
    }

    [Fact]
    public void Part2_WorksForRealData()
    {
        var dataStreamBuffer = ParseInput(FileHelper.ReadFromFile("Day6", "RealData.txt"));
        Assert.Equal(1794, Day6Puzzle.GetNumberOfCharactersProcessedBeforeFirstStartOfPacketMarker(dataStreamBuffer));
    }
    
    static DataStreamBuffer ParseInput(string input)
    {
        return new DataStreamBuffer(input.Select(c => c).ToArray());
    }
}