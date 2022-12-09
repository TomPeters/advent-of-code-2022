namespace AdventOfCode.Day8;

public class Day8Puzzle
{
    public static int GetNumberOfVisibleTrees(Forest forest)
    {
        return forest.GetAllVisibleTrees().Count();
    }

    public static int GetHighestScenicScore(Forest forest)
    {
        return forest.GetHighestScenicScore();
    }
}

public record Forest(Tree[] Trees)
{
    public IEnumerable<Tree> GetAllVisibleTrees()
    {
        var allDirections = (Direction[])Enum.GetValues(typeof(Direction));
        return Trees.Where(t => allDirections.Any(t.IsVisibleFromDirection));
    }

    public int GetHighestScenicScore()
    {
        return Trees.Select(t => t.GetScenicScore()).Max();
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
        var treesInDirection = GetAllTreesFromDirection(direction);
        return treesInDirection.All(t => t._height < _height);
    }

    IEnumerable<Tree> GetAllTreesFromDirection(Direction direction)
    {
        var hasAdjacentTree = _adjacentTrees.TryGetValue(direction, out var adjacentTree);
        if (!hasAdjacentTree) return Enumerable.Empty<Tree>();
        return new[] { adjacentTree! }.Concat(adjacentTree!.GetAllTreesFromDirection(direction));
    }

    public int GetScenicScore()
    {
        var allDirections = (Direction[])Enum.GetValues(typeof(Direction));
        return allDirections.Select(GetViewingDistance).Aggregate(1, (p, c) => p * c);
    }

    int GetViewingDistance(Direction direction)
    {
        var allTreesInDirection = GetAllTreesFromDirection(direction).ToArray();
        var count = allTreesInDirection.TakeWhile(t => t._height < _height).Count();
        
        // If we didn't reach the edge, then we need to additionally count the last one we found
        if (count < allTreesInDirection.Count()) return count + 1;
        return count;
    }
}

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}