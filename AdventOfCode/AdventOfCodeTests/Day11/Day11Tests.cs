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
        Assert.Equal(10605, Day11Puzzle.GetLevelOfMonkeyBusiness(GetSampleMonkeys(), 20));
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

    static Monkey CreateMonkey(int monkeyId, IEnumerable<int> startingWorryLevels, IOperation operation,
        int testDivisor, int trueMonkeyId, int falseMonkeyId)
    {
        return new Monkey(monkeyId, startingWorryLevels.Select(l => new Item(l)).ToArray(), operation,
            new NextMonkeyTestParams(testDivisor, trueMonkeyId, falseMonkeyId));
    }
}