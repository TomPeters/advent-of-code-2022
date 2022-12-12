namespace AdventOfCode.Day11;

public class Day11Puzzle
{
    public static int GetLevelOfMonkeyBusiness(Monkeys monkeys, int numRounds)
    {
        Enumerable.Range(0, numRounds).ForEach(_ => monkeys.PerformRound());
        return monkeys.GetMonkeyBusiness();
    }
}

public record Monkeys(Monkey[] monkeys)
{
    public void PerformRound() => monkeys.ForEach(m => m.PerformTurn(this));

    public Monkey GetMonkeyWithId(int monkeyId) => monkeys.Single(m => m.MonkeyId == monkeyId);

    public int GetMonkeyBusiness()
    {
        return monkeys.Select(m => m.NumberOfInspections).OrderByDescending(i => i).Take(2).Product();
    }
}

public class Monkey
{
    public int MonkeyId { get; }
    private Queue<Item> _items;
    private readonly IOperation _operation;
    private readonly NextMonkeyTestParams _nextMonkeyTestParams;
    public int NumberOfInspections { get; private set; } = 0;

    public Monkey(int monkeyId, Item[] startingItems, IOperation operation, NextMonkeyTestParams nextMonkeyTestParams)
    {
        MonkeyId = monkeyId;
        _items = new Queue<Item>(startingItems);
        _operation = operation;
        _nextMonkeyTestParams = nextMonkeyTestParams;
    }

    public void PerformTurn(Monkeys monkeys)
    {
        while (_items.TryDequeue(out var item))
        {
            InspectItem(item, monkeys);
        }
    }

    void InspectItem(Item item, Monkeys monkeys)
    {
        _operation.AdjustWorryLevel(item);
        GetBoredWithItem(item);
        var nextMonkey = new NextMonkeyTest(monkeys).GetNextMonkeyToThrowItemTo(item, _nextMonkeyTestParams);
        ThrowItemTo(item, nextMonkey);
        NumberOfInspections++;
    }

    private void ThrowItemTo(Item item, Monkey nextMonkey)
    {
        nextMonkey._items.Enqueue(item);
    }

    void GetBoredWithItem(Item item)
    {
        new SenseOfReliefOperation().AdjustWorryLevel(item);
    }
}

public class AddOperation : IOperation
{
    private readonly int _numberToAdd;

    public AddOperation(int numberToAdd)
    {
        _numberToAdd = numberToAdd;
    }

    public void AdjustWorryLevel(Item item)
    {
        item.WorryLevel += _numberToAdd;
    }
}

public class MultiplyOperation : IOperation
{
    private readonly int _factor;

    public MultiplyOperation(int factor)
    {
        _factor = factor;
    }

    public void AdjustWorryLevel(Item item)
    {
        item.WorryLevel *= _factor;
    }
}

public class SquareOperation : IOperation
{
    public void AdjustWorryLevel(Item item)
    {
        item.WorryLevel *= item.WorryLevel;
    }
}

public class SenseOfReliefOperation : IOperation
{
    public void AdjustWorryLevel(Item item)
    {
        item.WorryLevel /= 3;
    }
}

public interface IOperation
{
    void AdjustWorryLevel(Item item);
}

public class NextMonkeyTest
{
    private readonly Monkeys _monkeys;

    public NextMonkeyTest(Monkeys monkeys)
    {
        _monkeys = monkeys;
    }

    public Monkey GetNextMonkeyToThrowItemTo(Item item, NextMonkeyTestParams nextMonkeyTestParams)
    {
        var nextMonkeyId = item.WorryLevel % nextMonkeyTestParams.Divisor == 0
            ? nextMonkeyTestParams.TrueMonkeyId
            : nextMonkeyTestParams.FalseMonkeyId;
        return _monkeys.GetMonkeyWithId(nextMonkeyId);
    }
}

public record NextMonkeyTestParams(int Divisor, int TrueMonkeyId, int FalseMonkeyId);

public class Item
{
    public Item(int startingWorryLevel)
    {
        WorryLevel = startingWorryLevel;
    }
    
    public int WorryLevel { get; set; }
}