namespace AdventOfCode.Day10;

public class Day10Puzzle
{
    public static int GetSumOfSignalStrengths(IEnumerable<IInstruction> instructions, IEnumerable<int> cycleNumbersToSum)
    {
        var cpu = new Cpu(instructions);
        return cycleNumbersToSum.Select(cycleNumber =>
        {
            var registerDuringTargetCycle = cpu.AdvanceToCycle(cycleNumber);
            return registerDuringTargetCycle * cycleNumber;
        }).Sum();
    }
}

public class Cpu : IInstructionApi
{
    private readonly Queue<IInstruction> _instructions;
    private int _currentCycle = 0;
    private IInstruction? _currentlyExecutingInstruction;

    public Cpu(IEnumerable<IInstruction> instructions)
    {
        _instructions = new Queue<IInstruction>(instructions);
    }

    public int AdvanceToCycle(int targetCycle)
    {
        int? registerValueDuringTargetCycle = null;
        while (_currentCycle != targetCycle)
        {
            _currentCycle++;
            var nextInstruction = _currentlyExecutingInstruction ?? _instructions.Dequeue();
            _currentlyExecutingInstruction = null;
            nextInstruction.ExecuteForOneCycle();
            registerValueDuringTargetCycle = Register;
            nextInstruction.OnCycleComplete(this);
            if (!nextInstruction.IsComplete())
            {
                _currentlyExecutingInstruction = nextInstruction;
            }
        }

        if (!registerValueDuringTargetCycle.HasValue)
        {
            throw new Exception("Didn't reach target cycle");
        }

        return registerValueDuringTargetCycle.Value;
    }

    public int Register { get; set; } = 1;
}

public interface IInstructionApi
{
    public int Register { get; set; }
}

public interface IInstruction
{
    public void ExecuteForOneCycle();
    public void OnCycleComplete(IInstructionApi instructionApi);
    public bool IsComplete();
}

public class AddXInstruction : IInstruction
{
    private readonly int _amount;
    private int _numberOfCyclesRemaining = 2;

    public AddXInstruction(int amount)
    {
        _amount = amount;
    }

    public void ExecuteForOneCycle() => _numberOfCyclesRemaining--;

    public void OnCycleComplete(IInstructionApi instructionApi)
    {
        if (_numberOfCyclesRemaining == 0)
        {
            instructionApi.Register += _amount;
        }
    }

    public bool IsComplete() => _numberOfCyclesRemaining == 0;
}

public class NoOpInstruction : IInstruction
{
    private bool _isComplete;
    public void ExecuteForOneCycle() => _isComplete = true;
    public void OnCycleComplete(IInstructionApi instructionApi)
    {
    }

    public bool IsComplete() => _isComplete;
}