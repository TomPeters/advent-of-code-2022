using System.Numerics;

namespace AdventOfCode.Day11;

public class Day11Puzzle
{
    public static long GetLevelOfMonkeyBusiness(Monkeys monkeys, ISenseOfReliefOperation senseOfReliefOperation, int numRounds)
    {
        Enumerable.Range(0, numRounds).ForEach(_ => monkeys.PerformRound(senseOfReliefOperation));
        return monkeys.GetMonkeyBusiness();
    }
}

public record Monkeys(Monkey[] AllMonkeys)
{
    public void PerformRound(ISenseOfReliefOperation senseOfReliefOperation) => AllMonkeys.ForEach(m => m.PerformTurn(this, senseOfReliefOperation));

    public Monkey GetMonkeyWithId(int monkeyId) => AllMonkeys.Single(m => m.MonkeyId == monkeyId);

    public long GetMonkeyBusiness()
    {
        return AllMonkeys.Select(m => m.NumberOfInspections).OrderByDescending(i => i).Take(2).Product();
    }
}

public class Monkey
{
    public int MonkeyId { get; }
    private Queue<Item> _items;
    private readonly IOperation _operation;
    public readonly NextMonkeyTestParams NextMonkeyTestParams;
    public long NumberOfInspections { get; private set; } = 0L;

    public Monkey(int monkeyId, Item[] startingItems, IOperation operation, NextMonkeyTestParams nextMonkeyTestParams)
    {
        MonkeyId = monkeyId;
        _items = new Queue<Item>(startingItems);
        _operation = operation;
        NextMonkeyTestParams = nextMonkeyTestParams;
    }

    public void PerformTurn(Monkeys monkeys, ISenseOfReliefOperation senseOfReliefOperation)
    {
        while (_items.TryDequeue(out var item))
        {
            InspectItem(item, monkeys, senseOfReliefOperation);
        }
    }

    void InspectItem(Item item, Monkeys monkeys, ISenseOfReliefOperation senseOfReliefOperation)
    {
        _operation.AdjustWorryLevel(item);
        GetBoredWithItem(item, senseOfReliefOperation, monkeys);
        var nextMonkey = new NextMonkeyTest(monkeys).GetNextMonkeyToThrowItemTo(item, NextMonkeyTestParams);
        ThrowItemTo(item, nextMonkey);
        NumberOfInspections++;
    }

    private void ThrowItemTo(Item item, Monkey nextMonkey)
    {
        nextMonkey._items.Enqueue(item);
    }

    void GetBoredWithItem(Item item, ISenseOfReliefOperation senseOfReliefOperation, Monkeys monkeys)
    {
        senseOfReliefOperation.AdjustWorryLevel(item, monkeys);
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
        checked
        {
            item.WorryLevel += _numberToAdd;
        }
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
        checked
        {
            item.WorryLevel *= _factor;
        }
    }
}

public class SquareOperation : IOperation
{
    public void AdjustWorryLevel(Item item)
    {
        checked
        {
            item.WorryLevel *= item.WorryLevel;
        }
    }
}

public interface ISenseOfReliefOperation
{
    void AdjustWorryLevel(Item item, Monkeys monkeys);
}

public class ModuloReducedWorryOperation : ISenseOfReliefOperation
{
    public void AdjustWorryLevel(Item item, Monkeys monkeys)
    {
        // There's probably a mathematical identity this represents, but I don't what what it is :)
        // The number we store here should give us the same result when mod'ed with these divisors as if we had stored the original larger number.
        // This wouldn't work if we were doing different kinds of operations when we increase the worry level, like division.
        var productOfAllDivisors = monkeys.AllMonkeys.Select(m => m.NextMonkeyTestParams.Divisor).Product();
        item.WorryLevel %= productOfAllDivisors;
    }
}

public class ReducedWorryOperation : ISenseOfReliefOperation
{
    public void AdjustWorryLevel(Item item, Monkeys monkeys)
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
    
    public long WorryLevel { get; set; }
}