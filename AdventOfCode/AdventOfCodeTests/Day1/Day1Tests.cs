using System;
using System.Linq;
using AdventOfCode.Day1;
using Xunit;

namespace AdventOfCodeTests.Day1;

public class Day1Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var sampleData = ParseInput(FileHelper.ReadFromFile("Day1", "Sample.txt"));
        Assert.Equal(24000, Day1Puzzle.GetHighestCalorieCountForASingleElf(sampleData));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var sampleData = ParseInput(FileHelper.ReadFromFile("Day1", "RealData.txt"));
        Assert.Equal(71502, Day1Puzzle.GetHighestCalorieCountForASingleElf(sampleData));
    }
    
    [Fact]
    public void Part2_WorksForSampleData()
    {
        var sampleData = ParseInput(FileHelper.ReadFromFile("Day1", "Sample.txt"));
        Assert.Equal(45000, Day1Puzzle.GetTotalCaloriesCarriedByTop3Elves(sampleData));
    }
    
    [Fact]
    public void Part2_WorksForRealData()
    {
        var sampleData = ParseInput(FileHelper.ReadFromFile("Day1", "RealData.txt"));
        Assert.Equal(208191, Day1Puzzle.GetTotalCaloriesCarriedByTop3Elves(sampleData));
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