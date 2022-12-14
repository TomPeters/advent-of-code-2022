using System.Linq;
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