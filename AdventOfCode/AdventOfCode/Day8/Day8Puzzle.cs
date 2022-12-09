namespace AdventOfCode.Day8;

public class Day8Puzzle
{
    public static int GetNumberOfVisibleTrees(Forest forest)
    {
        var heights = forest.GetAllVisibleTrees().Select(t => t.Height).ToArray();
        return forest.GetAllVisibleTrees().Count();
    }
}

public record Forest(Tree[] Trees)
{
    public IEnumerable<Tree> GetAllVisibleTrees()
    {
        var allDirections = (Direction[])Enum.GetValues(typeof(Direction));
        return Trees.Where(t => allDirections.Any(t.IsVisibleFromDirection));
    }
}

public class Tree
{
    private readonly int _height;
    private readonly Dictionary<Direction, Tree> _adjacentTrees = new ();
    
    public Tree(int height)
    {
        _height = height;
    }

    public string Height => _height.ToString();

    public void RegisterAdjacentTree(Tree tree, Direction direction)
    {
        _adjacentTrees[direction] = tree;
    }

    public bool IsVisibleFromDirection(Direction direction)
    {
        var treesInDirection = GetAllTreesFromDirection(direction);
        return treesInDirection.All(t => t._height < _height);
    }

    IEnumerable<Tree> GetAllTreesFromDirection(Direction direction)
    {
        var hasAdjacentTree = _adjacentTrees.TryGetValue(direction, out var adjacentTree);
        if (!hasAdjacentTree) return Enumerable.Empty<Tree>();
        return new[] { adjacentTree! }.Concat(adjacentTree!.GetAllTreesFromDirection(direction));
    }
}

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}