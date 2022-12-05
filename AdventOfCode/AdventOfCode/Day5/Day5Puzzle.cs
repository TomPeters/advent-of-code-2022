namespace AdventOfCode.Day5;

public class Day5Puzzle
{
    public static string GetTopCratesAsString(Supplies supplies, Instruction[] instructions)
    {
        instructions.ForEach(supplies.ApplyInstruction);
        return new string(supplies.Stacks.Select(s => s.Stack.Peek()).ToArray());
    }
}

public record Supplies(SupplyStack[] Stacks)
{
    public void ApplyInstruction(Instruction instruction)
    {
        var sourceStack = Stacks.Single(s => s.StackName == instruction.SourceStackName);
        var destination = Stacks.Single(s => s.StackName == instruction.DestinationStackName);

        sourceStack.MoveItemsToStack(destination, instruction.NumberToMove);
    }
}

public record SupplyStack(Stack<char> Stack, string StackName)
{
    public void MoveItemsToStack(SupplyStack otherStack, int numberOfItemsToMove)
    {
        Enumerable.Range(0, numberOfItemsToMove).ForEach(_ =>
        {
            var item = Stack.Pop();
            otherStack.Stack.Push(item);
        });
    }
}

public record Instruction(string SourceStackName, string DestinationStackName, int NumberToMove);