namespace AdventOfCode.Day5;

public class Day5Puzzle
{
    public static string GetTopCratesAsString(Supplies supplies, Instruction[] instructions, ICrane crane)
    {
        instructions.ForEach(i => supplies.ApplyInstruction(i, crane));
        return new string(supplies.Stacks.Select(s => s.Stack.Peek()).ToArray());
    }
}

public record Supplies(SupplyStack[] Stacks)
{
    public void ApplyInstruction(Instruction instruction, ICrane crane)
    {
        var sourceStack = Stacks.Single(s => s.StackName == instruction.SourceStackName);
        var destination = Stacks.Single(s => s.StackName == instruction.DestinationStackName);

        crane.MoveItemsBetweenStacks(sourceStack, destination, instruction.NumberToMove);
    }
}

public record SupplyStack(Stack<char> Stack, string StackName);

public record Instruction(string SourceStackName, string DestinationStackName, int NumberToMove);

public interface ICrane
{
    void MoveItemsBetweenStacks(SupplyStack sourceStack, SupplyStack destinationStack, int numberToMove);
}

public class CrateMover9000 : ICrane
{
    public void MoveItemsBetweenStacks(SupplyStack sourceStack, SupplyStack destinationStack, int numberToMove)
    {
        Enumerable.Range(0, numberToMove).ForEach(_ =>
        {
            var item = sourceStack.Stack.Pop();
            destinationStack.Stack.Push(item);
        });
    }
}

public class CrateMover9001 : ICrane
{
    public void MoveItemsBetweenStacks(SupplyStack sourceStack, SupplyStack destinationStack, int numberToMove)
    {
        var itemsToMove = Enumerable.Range(0, numberToMove).Select(_ => sourceStack.Stack.Pop());
        itemsToMove.Reverse().ForEach(i => destinationStack.Stack.Push(i));
    }
}