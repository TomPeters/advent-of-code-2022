using Microsoft.VisualBasic;

namespace AdventOfCode.Day12;

public class Day12Puzzle
{
    public static int GetLengthOfShortestPath(Heightmap heightmap)
    {
        var shortestPath = heightmap.GetShortestPathFrom(heightmap.GetStartingSquare());
        return shortestPath!.NumberOfSteps;
    }
    
    public static int GetLengthOfShortestPathFromAnyPotentialStartingSquare(Heightmap heightmap)
    {
        return heightmap.GetShortestPathFromAllPotentialStartingSquares().Select(p => p.NumberOfSteps).Min();
    }
}

public class Heightmap
{
    private readonly Square[] _allSquares;

    public Heightmap(Square[] allSquares)
    {
        _allSquares = allSquares;
    }

    public IEnumerable<Path> GetShortestPathFromAllPotentialStartingSquares()
    {
        return _allSquares.Where(s => s.IsPotentialStartingSquare()).Select(GetShortestPathFrom).Where(p => p != null);
    }

    public Square GetStartingSquare() => _allSquares.Single(s => s.IsStart());

    public Path? GetShortestPathFrom(Square firstSquare)
    {
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
                    if (existingShortestPath.NumberOfSteps > nextPath.NumberOfSteps)
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
        return !shortestPaths.ContainsKey(endSquare) ? null : shortestPaths[endSquare];
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

    public int NumberOfSteps => _squares.Length - 1;
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
    
    char Elevation => IsEnd() ? 'z' : IsStart() ? 'a' : _height;
    
    private bool IsValid(Square adjacentSquare)
    {
        if (IsEnd()) return false; // Once we get to the end, we don't have to go anywhere else
        return adjacentSquare.Elevation <= Elevation + 1;
    }

    public bool IsPotentialStartingSquare()
    {
        return IsStart() || _height == 'a';
    }

    public bool IsStart() => _height == 'S';
    public bool IsEnd() => _height == 'E';
}