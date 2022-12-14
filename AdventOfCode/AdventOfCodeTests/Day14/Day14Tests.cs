using System.Linq;
using AdventOfCode.Day14;
using Xunit;

namespace AdventOfCodeTests.Day14;

public class Day14Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day14", "Sample.txt"));
        Assert.Equal(24, Day14Puzzle.GetUnitsOfSandThatFlowBeforeTheRestFlowIntoTheAbyss(input, new FallsForeverCondition(), false));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day14", "RealData.txt"));
        Assert.Equal(614, Day14Puzzle.GetUnitsOfSandThatFlowBeforeTheRestFlowIntoTheAbyss(input, new FallsForeverCondition(), false));
    }
    
    [Fact]
    public void Part2_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day14", "Sample.txt"));
        Assert.Equal(93, Day14Puzzle.GetUnitsOfSandThatFlowBeforeTheRestFlowIntoTheAbyss(input, new PilesUpToSandSourceCoordinateCondition(), true));
    }
        
    [Fact]
    public void Part2_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day14", "RealData.txt"));
        Assert.Equal(93, Day14Puzzle.GetUnitsOfSandThatFlowBeforeTheRestFlowIntoTheAbyss(input, new PilesUpToSandSourceCoordinateCondition(), true));
    }

    private AllRockPaths ParseInput(string input)
    {
        var rockPaths = input.Split("\n").Select(rockPathInput =>
        {
            var coordinates = rockPathInput.Split("->").Select(i => i.Trim()).Select(coordinates =>
            {
                var coordInputs = coordinates.Split(",").Select(int.Parse).ToArray();
                return new Coordinate(coordInputs[0], coordInputs[1]);
            }).ToArray();
            var rockLines = coordinates.Zip(coordinates.Skip(1), (c1, c2) => new RockLine(c1, c2)).ToArray();
            return new RockPath(rockLines);
        }).ToArray();
        return new AllRockPaths(rockPaths);
    }
}