using System;
using System.Linq;
using AdventOfCode.Day1;
using Xunit;

namespace AdventOfCodeTests.Day1;

public class Day1Tests
{
    [Fact]
    public void WorksForSampleData()
    {
        var sampleData = ParseInput(FileHelper.ReadFromFile("Day1", "Sample.txt"));
        Assert.Equal(24000, Day1Puzzle.GetResult(sampleData));
    }
    
    [Fact]
    public void WorksForRealData()
    {
        var sampleData = ParseInput(FileHelper.ReadFromFile("Day1", "RealData.txt"));
        Assert.Equal(71502, Day1Puzzle.GetResult(sampleData));
    }

    public Elf[] ParseInput(string input)
    {
        return input.Split("\n\n")
            .Select(elfInput =>
            {
                var food = elfInput.Split("\n").Select(calories => new Food(Int32.Parse(calories)));
                return new Elf(food.ToArray());
            }).ToArray();
    }
}