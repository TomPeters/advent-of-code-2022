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
        var input = ParseInput(FileHelper.ReadFromFile("Day3", "Sample.txt"));
        Assert.Equal(157, Day3Puzzle.GetSumOfPriorityOfSingleContentInBothCompartmentsOfEachRucksack(input));
    }
    
    [Fact]
    public void Part2_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day3", "RealData.txt"));
        Assert.Equal(7793, Day3Puzzle.GetSumOfPriorityOfSingleContentInBothCompartmentsOfEachRucksack(input));
    }

    static Rucksack[] ParseInput(string input)
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