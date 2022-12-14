namespace AdventOfCode.Day14;

public class Day14Puzzle
{
    public static int GetUnitsOfSandThatFlowBeforeTheRestFlowIntoTheAbyss(AllRockPaths allRockPaths, ISandCondition sandCondition, bool hasFloor)
    {
        var grid = new Grid(allRockPaths, hasFloor);
        var sandSource = new SandSource(grid);
        while (sandSource.DropSand(sandCondition) == SandDropResult.CameToRest)
        {
        }

        return sandSource.CountOfSandDropped;
    }
}

public interface ISandCondition
{
    bool AllRequiredSandHasBeenDropped(Sand sand);
}

public class FallsForeverCondition : ISandCondition
{
    public bool AllRequiredSandHasBeenDropped(Sand sand) => sand.IsFallingForever();
}

public class PilesUpToSandSourceCoordinateCondition : ISandCondition
{
    public bool AllRequiredSandHasBeenDropped(Sand sand) => sand.Coordinate.Equals(SandSource.SourceCoord);
}

public class SandSource
{
    private readonly Grid _grid;
    public static readonly Coordinate SourceCoord = new(500, 0);
    public int CountOfSandDropped { get; private set; } = 0;

    public SandSource(Grid grid)
    {
        _grid = grid;
    }

    public SandDropResult DropSand(ISandCondition condition)
    {
        var sand = new Sand(SourceCoord, _grid);
        while (sand.TryMove())
            // In the case things falling infinitely, the sand can always be moved
            if (condition.AllRequiredSandHasBeenDropped(sand))
                return SandDropResult.FinalSandDropped;

        
        CountOfSandDropped++;

        // In the case of an infinite floor, the sand can't move when it reaches this condition
        if (condition.AllRequiredSandHasBeenDropped(sand))
            return SandDropResult.FinalSandDropped;
        
        _grid.AddRestingSand(sand);

        return SandDropResult.CameToRest;
    }
}

public enum SandDropResult
{
    CameToRest,
    FinalSandDropped
}

public class Grid
{
    private readonly bool _hasFloor;
    private readonly HashSet<Coordinate> _allRockAndSandCoordinates;
    private readonly List<Sand> _allSand = new List<Sand>();
    private readonly int _lowestYCoord;

    public Grid(AllRockPaths allRockPaths, bool hasFloor)
    {
        _hasFloor = hasFloor;
        var allRockCoordinates = allRockPaths.AllCoordinatesThatContainRocks().ToArray();
        _lowestYCoord = allRockCoordinates.Select(c => c.Y).Max();
        _allRockAndSandCoordinates = new HashSet<Coordinate>(allRockCoordinates);
    }

    public bool IsViableNextPositionForSand(Coordinate coordinate)
    {
        if (_hasFloor && coordinate.Y >= _lowestYCoord + 2)
        {
            // It has hit the infinite floor
            return false;
        }
        return !_allRockAndSandCoordinates.Contains(coordinate);
    }

    public void AddRestingSand(Sand sand) => _allRockAndSandCoordinates.Add(sand.Coordinate);

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