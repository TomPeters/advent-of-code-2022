using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Day10;
using Xunit;

namespace AdventOfCodeTests.Day10;

public class Day10Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day10", "Sample.txt"));
        Assert.Equal(13140, Day10Puzzle.GetSumOfSignalStrengths(input, GetCyclesToSum()));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day10", "RealData.txt"));
        Assert.Equal(16480, Day10Puzzle.GetSumOfSignalStrengths(input, GetCyclesToSum()));
    }

    static IEnumerable<int> GetCyclesToSum() => Enumerable.Range(1, 6).Select(i => i * 40 - 20);

    static IEnumerable<IInstruction> ParseInput(string input)
    {
        return input.Split("\n").Select<string, IInstruction>(instructionInput =>
        {
            return instructionInput switch
            {
                "noop" => new NoOpInstruction(),
                _ => new AddXInstruction(int.Parse(instructionInput.Split(" ")[1]))
            };
        });
    }
}