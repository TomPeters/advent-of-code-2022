using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode;
using AdventOfCode.Day5;
using Xunit;

namespace AdventOfCodeTests.Day5;

public class Day5Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var (supplies, instructions) = ParseInput(FileHelper.ReadFromFile("Day5", "Sample.txt"));
        Assert.Equal("CMZ", Day5Puzzle.GetTopCratesAsString(supplies, instructions));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var (supplies, instructions) = ParseInput(FileHelper.ReadFromFile("Day5", "RealData.txt"));
        Assert.Equal("QNHWJVJZW", Day5Puzzle.GetTopCratesAsString(supplies, instructions));
    }

    static (Supplies supplies, Instruction[] instructions) ParseInput(string input)
    {
        var split = input.Split("\n\n");
        return (ParseSupplies(split[0]), ParseInstructions(split[1]));
    }

    static Supplies ParseSupplies(string input)
    {
        var transposed = input.Transpose();
        var trimmedRowsInput = string.Join("\n", transposed.Split("\n").Select(i => i.Trim()));
        var stacks = trimmedRowsInput.Split("\n\n").Select(ParseSupplyStack).ToArray();
        return new Supplies(stacks);
    }

    static SupplyStack ParseSupplyStack(string stackInput)
    {
        var stackInputRow = stackInput.Split("\n")[1].Trim().Reverse().ToArray();
        var stackInputName = stackInputRow.First();
        var stackItems = stackInputRow.Skip(1);
        var stack = new Stack<char>(stackItems);
        return new SupplyStack(stack, stackInputName.ToString());
    }

    static Instruction[] ParseInstructions(string input)
    {
        return input.Split("\n").Select(i =>
        {
            var parts = i.Split(" from ");
            var numberToMove = int.Parse(parts[0].Split("move ", StringSplitOptions.RemoveEmptyEntries)[0]);
            var stacks = parts[1].Split(" to ");
            var sourceStack = stacks[0];
            var destinationStack = stacks[1];
            return new Instruction(sourceStack, destinationStack, numberToMove);
        }).ToArray();
    }
}