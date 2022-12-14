namespace AdventOfCode.Day14;

public class Day14Puzzle
{
    public static int GetUnitsOfSandThatFlowBeforeTheRestFlowIntoTheAbyss(AllRockPaths allRockPaths)
    {
        var grid = new Grid(allRockPaths);
        var sandSource = new SandSource(grid);
        while (sandSource.DropSand() == SandDropResult.CameToRest)
        {
        }

        return sandSource.CountOfSandDropped;
    }
}

public class SandSource
{
    private readonly Grid _grid;
    static readonly Coordinate SourceCoord = new(500, 0);
    public int CountOfSandDropped { get; private set; } = 0;

    public SandSource(Grid grid)
    {
        _grid = grid;
    }

    public SandDropResult DropSand()
    {
        var sand = new Sand(SourceCoord, _grid);
        _grid.AddSand(sand);
        while (sand.TryMove())
            if (sand.IsFallingForever())
                return SandDropResult.FallsForever;

        CountOfSandDropped++;
        return SandDropResult.CameToRest;
    }
}

public enum SandDropResult
{
    CameToRest,
    FallsForever
}

public class Grid
{
    private readonly Coordinate[] _rockCoordinates;
    private readonly List<Sand> _allSand = new List<Sand>();
    private readonly int _lowestYCoord;

    public Grid(AllRockPaths allRockPaths)
    {
        _rockCoordinates = allRockPaths.AllCoordinatesThatContainRocks().ToArray();
        _lowestYCoord = _rockCoordinates.Select(c => c.Y).Max();
    }

    public bool IsViableNextPositionForSand(Coordinate coordinate)
    {
        return !_rockCoordinates.Concat(_allSand.Select(s => s.Coordinate)).Contains(coordinate);
    }

    public void AddSand(Sand sand) => _allSand.Add(sand);

    public bool IsBelowAllRock(Coordinate coordinate)
    {
        return coordinate.Y > _lowestYCoord;
    }
}

public class Sand
{
    public Coordinate Coordinate { get; private set; }
    private readonly Grid _grid;

    public Sand(Coordinate coordinate, Grid grid)
    {
        Coordinate = coordinate;
        _grid = grid;
    }
    
    public bool TryMove()
    {
        var possibleNextPositions = new[]
        {
            Coordinate with { Y = Coordinate.Y + 1 },
            Coordinate with { Y = Coordinate.Y + 1, X = Coordinate.X - 1 },
            Coordinate with { Y = Coordinate.Y + 1, X = Coordinate.X + 1 },
        };
        var nextSandPosition = possibleNextPositions.Where(_grid.IsViableNextPositionForSand).FirstOrDefault();
        if (nextSandPosition is null)
        {
            return false;
        }

        Coordinate = nextSandPosition;
        return true;
    }

    public bool IsFallingForever() => _grid.IsBelowAllRock(Coordinate);
}

public record AllRockPaths(RockPath[] rockPaths)
{
    public IEnumerable<Coordinate> AllCoordinatesThatContainRocks()
    {
        return rockPaths.SelectMany(p => p.AllCoordinatesThatContainRocks()).Distinct();
    }
}

public record RockPath(RockLine[] rockLines)
{
    public IEnumerable<Coordinate> AllCoordinatesThatContainRocks()
    {
        return rockLines.SelectMany(l => l.AllCoordinatesThatContainRocks()).Distinct();
    }
}

public record RockLine(Coordinate startCoordinate, Coordinate endCoordinate)
{
    public IEnumerable<Coordinate> AllCoordinatesThatContainRocks()
    {
        var allCoords = new[] { startCoordinate, endCoordinate };
        var lowestX = allCoords.Select(c => c.X).Min();
        var lowestY = allCoords.Select(c => c.Y).Min();
        var highestX = allCoords.Select(c => c.X).Max();
        var highestY = allCoords.Select(c => c.Y).Max();
        var dx = highestX - lowestX;
        var dy = highestY - lowestY;
        var xRange = Enumerable.Range(lowestX, dx + 1);
        var yRange = Enumerable.Range(lowestY, dy + 1);
        return xRange.SelectMany(x => yRange.Select(y => new Coordinate(x, y)));
    }
}

public record Coordinate(int X, int Y);