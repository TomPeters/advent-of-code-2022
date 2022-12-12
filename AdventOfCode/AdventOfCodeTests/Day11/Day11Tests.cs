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
            new Monkey(0, new[] { new Item(79), new Item(98) }, new MultiplyOperation(19),
                new NextMonkeyTestParams(23, 2, 3)),
            new Monkey(1, new[] { new Item(54), new Item(65), new Item(75), new Item(74) }, new AddOperation(6),
                new NextMonkeyTestParams(19, 2, 0)),
            new Monkey(2, new[] { new Item(79), new Item(60), new Item(97) }, new SquareOperation(),
                new NextMonkeyTestParams(13, 1, 3)),
            new Monkey(3, new[] { new Item(74) }, new AddOperation(3), new NextMonkeyTestParams(17, 0, 1)),
        };
        return new Monkeys(monkeys);
    }
}