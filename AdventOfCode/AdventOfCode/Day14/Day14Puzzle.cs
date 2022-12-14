namespace AdventOfCode.Day14;

public class Day14Puzzle
{
    public static int GetUnitsOfSandThatFlowBeforeTheRestFlowIntoTheAbyss(AllRockPaths allRockPaths)
    {
        return 0;
    }
}

public record AllRockPaths(RockPath[] rockPaths);

public record RockPath(RockLine[] rockLines);

public record RockLine(Coordinate startCoordinate, Coordinate endCoordinate);

public record Coordinate(int X, int Y);