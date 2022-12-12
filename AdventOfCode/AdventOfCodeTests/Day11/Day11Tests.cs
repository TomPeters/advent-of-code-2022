using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Day11;
using Xunit;

namespace AdventOfCodeTests.Day11;

public class Day11Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        Assert.Equal(10605, Day11Puzzle.GetLevelOfMonkeyBusiness(GetSampleMonkeys(), new ReducedWorryOperation(), 20));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        Assert.Equal(95472, Day11Puzzle.GetLevelOfMonkeyBusiness(GetRealMonkeys(), new ReducedWorryOperation(), 20));
    }
    
    [Fact]
    public void Part2_WorksForSampleData()
    {
        Assert.Equal(2713310158L, Day11Puzzle.GetLevelOfMonkeyBusiness(GetSampleMonkeys(), new NoReducedWorryOperation(), 10000));
    }

    Monkeys GetSampleMonkeys()
    {
        var monkeys = new[]
        {
            CreateMonkey(0, new []{79, 98}, new MultiplyOperation(19),
                23, 2, 3),
            CreateMonkey(1, new []{54, 65, 75, 74}, new AddOperation(6),
                19, 2, 0),
            CreateMonkey(2, new []{79, 60, 97}, new SquareOperation(),
                13, 1, 3),
            CreateMonkey(3, new []{74}, new AddOperation(3), 17, 0, 1),
        };
        return new Monkeys(monkeys);
    }

    Monkeys GetRealMonkeys()
    {
        var monkeys = new[]
        {
            CreateMonkey(0, new[] { 52, 60, 85, 69, 75, 75 }, new MultiplyOperation(17), 13, 6, 7),
            CreateMonkey(1, new[] { 96, 82, 61, 99, 82, 84, 85 }, new AddOperation(8), 7, 0, 7),
            CreateMonkey(2, new[] { 95, 79 }, new AddOperation(6), 19, 5, 3),
            CreateMonkey(3, new[] { 88, 50, 82, 65, 77 }, new MultiplyOperation(19), 2, 4, 1),
            CreateMonkey(4, new[] { 66, 90, 59, 90, 87, 63, 53, 88 }, new AddOperation(7), 5, 1, 0),
            CreateMonkey(5, new[] { 92, 75, 62 }, new SquareOperation(), 3, 3, 4),
            CreateMonkey(6, new[] { 94, 86, 76, 67 }, new AddOperation(1), 11, 5, 2),
            CreateMonkey(7, new[] { 57 }, new AddOperation(2), 17, 6, 2)
        };
        return new Monkeys(monkeys);
    }

    static Monkey CreateMonkey(int monkeyId, IEnumerable<int> startingWorryLevels, IOperation operation,
        int testDivisor, int trueMonkeyId, int falseMonkeyId)
    {
        return new Monkey(monkeyId, startingWorryLevels.Select(l => new Item(l)).ToArray(), operation,
            new NextMonkeyTestParams(testDivisor, trueMonkeyId, falseMonkeyId));
    }
}