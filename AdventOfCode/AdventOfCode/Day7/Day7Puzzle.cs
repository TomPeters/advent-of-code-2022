namespace AdventOfCode.Day7;

public class Day7Puzzle
{
    public static int GetSumOfSizeOfDirectoriesGreaterThan(string instructionsString, int sizeThreshold)
    {
        var rootDirectory = RootDirectory.CreateAndPopulateFromInstructions(ParseInstructions(instructionsString));
        
        var allDirectoriesBelowSizeThreshold = new[] { rootDirectory }
            .Concat(rootDirectory.GetAllDescendantDirectories())
            .Where(d => d.GetTotalSize() <= sizeThreshold);
        
        return allDirectoriesBelowSizeThreshold.Sum(d => d.GetTotalSize());
    }
    
    public static int GetSizeOfSmallestDirectoryToDelete(string instructionsString, int desiredFreeSpace, int totalFilesystemCapacity)
    {
        var rootDirectory = RootDirectory.CreateAndPopulateFromInstructions(ParseInstructions(instructionsString));

        var maximumAllowedRootDirectorySize = totalFilesystemCapacity - desiredFreeSpace;
        var currentRootDirectorySize = rootDirectory.GetTotalSize();
        var sizeSavingsRequired = currentRootDirectorySize - maximumAllowedRootDirectorySize;
        
        var directoryToDelete = new[] { rootDirectory }
            .Concat(rootDirectory.GetAllDescendantDirectories())
            .Where(d => d.GetTotalSize() > sizeSavingsRequired)
            .OrderBy(d => d.GetTotalSize())
            .First();
        
        return directoryToDelete.GetTotalSize();
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

public class RootDirectory : Directory
{
    public static RootDirectory CreateAndPopulateFromInstructions(IEnumerable<IInstruction> instructions)
    {
        var rootDirectory = new RootDirectory();
        Directory currentDirectory = rootDirectory;
        foreach (var instruction in instructions)
        {
            if (instruction is ChangeDirectoryInstruction changeDirectoryInstruction)
            {
                currentDirectory = changeDirectoryInstruction.Navigate(currentDirectory, rootDirectory);
            } else if (instruction is ListInstruction listInstruction)
            {
                listInstruction.RecordListResults(currentDirectory);
            }
            else
            {
                throw new Exception("Instruction not implemented");
            }
        }

        return rootDirectory;
    }
    
    RootDirectory() : base("/")
    {
    }
}

public class NonRootDirectory : Directory
{
    public Directory ParentDirectory { get; }

    public NonRootDirectory(string name, Directory parentDirectory) : base(name)
    {
        ParentDirectory = parentDirectory;
    }
}

public class Directory
{
    private readonly string _name;
    private readonly List<NonRootDirectory> _childDirectories = new();
    private readonly List<File> _files = new();
    
    public Directory(string name)
    {
        _name = name;
    }

    public IEnumerable<Directory> GetAllDescendantDirectories()
    {
        return _childDirectories.Concat(_childDirectories.SelectMany(c => c.GetAllDescendantDirectories()));
    }

    public int GetTotalSize()
    {
        return GetAllDescendantFiles().Sum(f => f.Size);
    }

    IEnumerable<File> GetAllDescendantFiles()
    {
        return GetAllDescendantDirectories().SelectMany(d => d._files).Concat(_files);
    }

    public void RecordChildDirectory(string directoryName)
    {
        GetChildDirectory(directoryName);
    }

    public Directory GetChildDirectory(string directoryName)
    {
        var knownDirectory = _childDirectories.FirstOrDefault(d => d._name == directoryName);
        if (knownDirectory is not null)
        {
            return knownDirectory;
        }

        var newDirectory = new NonRootDirectory(directoryName, this);
        _childDirectories.Add(newDirectory);
        return newDirectory;
    }

    public void RecordFile(string fileName, int fileSize)
    {
        _files.Add(new File(fileName, fileSize));
    }
}

public class File
{
    public string FileName { get; }
    public int Size { get; }

    public File(string fileName, int size)
    {
        FileName = fileName;
        Size = size;
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

    public Directory Navigate(Directory currentDirectory, Directory rootDirectory)
    {
        if (_argument == "/") return rootDirectory;
        if (_argument == "..")
        {
            if (currentDirectory is NonRootDirectory nonRootDirectory)
            {
                return nonRootDirectory.ParentDirectory;
            }

            throw new Exception("Tried to get parent of the root directory");
        }

        return currentDirectory.GetChildDirectory(_argument);
    }
}

public class ListInstruction : IInstruction
{
    private readonly IListResult[] _listResults;

    public ListInstruction(IListResult[] listResults)
    {
        _listResults = listResults;
    }

    public void RecordListResults(Directory directory)
    {
        foreach (var listResult in _listResults)
        {
            if (listResult is DirectoryListResult directoryListResult)
            {
                directory.RecordChildDirectory(directoryListResult.DirectoryName);
            } else if (listResult is FileListResult fileListResult)
            {
                directory.RecordFile(fileListResult.FileName, fileListResult.FileSize);
            }
        }
    }
}

public interface IListResult
{
}

public class DirectoryListResult : IListResult
{
    public string DirectoryName { get; }

    public DirectoryListResult(string directoryName)
    {
        DirectoryName = directoryName;
    }
}

public class FileListResult : IListResult
{
    public string FileName { get; }
    public int FileSize { get; }

    public FileListResult(string fileName, int fileSize)
    {
        FileName = fileName;
        FileSize = fileSize;
    }
}
