using System.Collections.Generic;
using System.Linq;
using AdventOfCode;
using Xunit;

namespace AdventOfCodeTests.Day3;

public class Day3Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var rucksacks = ParseInputAsRucksacks(FileHelper.ReadFromFile("Day3", "Sample.txt"));
        Assert.Equal(157, Day3Puzzle.GetSumOfPriorityOfSingleContentInBothCompartmentsOfEachRucksack(rucksacks));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var rucksacks = ParseInputAsRucksacks(FileHelper.ReadFromFile("Day3", "RealData.txt"));
        Assert.Equal(7793, Day3Puzzle.GetSumOfPriorityOfSingleContentInBothCompartmentsOfEachRucksack(rucksacks));
    }
    
    [Fact]
    public void Part2_WorksForSampleData()
    {
        var elfGroups = ParseInputAsElfGroups(FileHelper.ReadFromFile("Day3", "Sample.txt"));
        Assert.Equal(70, Day3Puzzle.GetSumOfEachGroupsBadgePriority(elfGroups));
    }
    
    [Fact]
    public void Part2_WorksForRealData()
    {
        var elfGroups = ParseInputAsElfGroups(FileHelper.ReadFromFile("Day3", "RealData.txt"));
        Assert.Equal(2499, Day3Puzzle.GetSumOfEachGroupsBadgePriority(elfGroups));
    }

    static ElfGroup[] ParseInputAsElfGroups(string input)
    {
        var rucksacks = ParseInputAsRucksacks(input);
        return rucksacks.Batch(3).Select(r => new ElfGroup(r.ToArray())).ToArray();
    }

    static Rucksack[] ParseInputAsRucksacks(string input)
    {
        return input.Split("\n").Select(rucksackInput =>
        {
            var firstRucksackContents = rucksackInput.Take(rucksackInput.Length / 2);
            var secondRucksackContents = rucksackInput.Skip(rucksackInput.Length / 2);
            var firstCompartment = CreateCompartment(firstRucksackContents);
            var secondCompartment = CreateCompartment(secondRucksackContents);
            return new Rucksack(firstCompartment, secondCompartment);
        }).ToArray();
    }

    static Compartment CreateCompartment(IEnumerable<char> compartmentContents)
    {
        return new Compartment(compartmentContents.Select(c => new Content(c)).ToArray());
    }
}