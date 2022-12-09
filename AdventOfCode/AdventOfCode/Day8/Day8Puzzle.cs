namespace AdventOfCode.Day8;

public class Day8Puzzle
{
    public static int GetNumberOfVisibleTrees(Forest forest)
    {
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

    public void RegisterAdjacentTree(Tree tree, Direction direction)
    {
        _adjacentTrees[direction] = tree;
    }

    public bool IsVisibleFromDirection(Direction direction)
    {
        var hasAdjacentTree = _adjacentTrees.TryGetValue(direction, out var adjacentTree);
        if (!hasAdjacentTree) return true;
        return adjacentTree!._height < _height && adjacentTree.IsVisibleFromDirection(direction);
    }
}

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}