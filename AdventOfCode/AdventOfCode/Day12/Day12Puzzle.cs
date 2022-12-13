using Microsoft.VisualBasic;

namespace AdventOfCode.Day12;

public class Day12Puzzle
{
    public static int GetLengthOfShortestPath(Heightmap heightmap)
    {
        return heightmap.GetShortestPathLength();
    }
}

public class Heightmap
{
    private readonly Square[] _allSquares;

    public Heightmap(Square[] allSquares)
    {
        _allSquares = allSquares;
    }

    public int GetShortestPathLength()
    {
        var firstSquare = _allSquares.Single(s => s.IsStart());
        var squaresToProcess = new Queue<Square>(new[] { firstSquare });
        var shortestPaths = new Dictionary<Square, Path> { { firstSquare, new Path(new []{ firstSquare }) } };
        while (squaresToProcess.TryDequeue(out var currentSquare))
        {
            var shortestPathToCurrentSquare = shortestPaths[currentSquare];
            foreach (var nextValidSquare in currentSquare.ValidSteps())
            {
                if (shortestPaths.ContainsKey(nextValidSquare)) continue;
                squaresToProcess.Enqueue(nextValidSquare);
                var nextPath = shortestPathToCurrentSquare.AddSquare(nextValidSquare);
                if (shortestPaths.ContainsKey(nextValidSquare))
                {
                    var existingShortestPath = shortestPaths[nextValidSquare];
                    if (existingShortestPath.Length > nextPath.Length)
                    {
                        shortestPaths[nextValidSquare] = nextPath;
                    }
                }
                else
                {
                    shortestPaths[nextValidSquare] = nextPath;
                }
            }
        }

        var endSquare = _allSquares.Single(s => s.IsEnd());
        return shortestPaths[endSquare].Length;
    }
}

public class Path
{
    private readonly Square[] _squares;

    public Path(Square[] squares)
    {
        _squares = squares;
    }

    public Path AddSquare(Square square)
    {
        return new Path(_squares.Concat(new[] { square }).ToArray());
    }

    public int Length => _squares.Length;
}

public class Square
{
    private readonly char _height;
    private List<Square> adjacentSquares = new();

    public Square(char height)
    {
        _height = height;
    }

    void AddAdjacentSquare(Square adjacentSquare)
    {
        adjacentSquares.Add(adjacentSquare);
    }

    public static void Connect(Square square1, Square square2)
    {
        square1.AddAdjacentSquare(square2);
        square2.AddAdjacentSquare(square1);
    }

    public IEnumerable<Square> ValidSteps()
    {
        return adjacentSquares.Where(IsValid);
    }

    private bool IsValid(Square adjacentSquare)
    {
        if (IsEnd()) return false;
        if (adjacentSquare.IsStart()) return false;
        return adjacentSquare.IsEnd() || _height <= adjacentSquare._height;
    }

    public bool IsStart() => _height == 'S';
    public bool IsEnd() => _height == 'E';
}