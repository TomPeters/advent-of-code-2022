using System.Linq;
using AdventOfCode.Day9;
using Xunit;

namespace AdventOfCodeTests.Day9;

public class Day9Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day9", "Sample.txt"));
        Assert.Equal(13, Day9Puzzle.GetNumberOfPositionsVisitedByRopeTail(input));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day9", "RealData.txt"));
        Assert.Equal(6256, Day9Puzzle.GetNumberOfPositionsVisitedByRopeTail(input));
    }

    static Input ParseInput(string input)
    {
        return new Input(input.Split("\n").Select(motionInput =>
        {
            var parts = motionInput.Split(" ");
            var direction = parts[0] switch
            {
                "R" => Direction.Right,
                "L" => Direction.Left,
                "U" => Direction.Up,
                "D" => Direction.Down
            };
            var numberOfSteps = int.Parse(parts[1]);
            return new Motion(direction, numberOfSteps);
        }).ToArray());
    }
}