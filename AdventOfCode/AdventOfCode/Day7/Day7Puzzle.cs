namespace AdventOfCode.Day7;

public class Day7Puzzle
{
    public static int GetSumOfSizeOfDirectoriesGreaterThan(string instructionsString, int sizeThreshold)
    {
        var instructions = ParseInstructions(instructionsString);
        return 0;
    }

    static IEnumerable<IInstruction> ParseInstructions(string instructions)
    {
        return instructions.Split("$", StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.Trim())
            .Select<string, IInstruction>(instructionString => {
                if (instructionString.StartsWith("cd"))
                {
                    return new ChangeDirectoryInstruction(instructionString.Split("cd ")[1]);
                }

                return ParseListInstruction(instructionString);
            });
    }

    static ListInstruction ParseListInstruction(string listInstructionInput)
    {
        var resultLines = listInstructionInput.Split("\n").Skip(1);
        return new ListInstruction(resultLines.Select<string, IListResult>(l =>
        {
            if (l.StartsWith("dir"))
            {
                return new DirectoryListResult(l.Split("dir ")[1]);
            }

            var items = l.Split(" ");
            return new FileListResult(items[1], int.Parse(items[0]));
        }).ToArray());
    }
}

public interface IInstruction
{
}

public class ChangeDirectoryInstruction : IInstruction
{
    private readonly string _argument;

    public ChangeDirectoryInstruction(string argument)
    {
        _argument = argument;
    }
}

public class ListInstruction : IInstruction
{
    private readonly IListResult[] _listResults;

    public ListInstruction(IListResult[] listResults)
    {
        _listResults = listResults;
    }
}

public interface IListResult
{
}

public class DirectoryListResult : IListResult
{
    private readonly string _directoryName;

    public DirectoryListResult(string directoryName)
    {
        _directoryName = directoryName;
    }
}

public class FileListResult : IListResult
{
    private readonly string _fileName;
    private readonly int _fileSize;

    public FileListResult(string fileName, int fileSize)
    {
        _fileName = fileName;
        _fileSize = fileSize;
    }
}
