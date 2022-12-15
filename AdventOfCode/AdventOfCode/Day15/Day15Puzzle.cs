namespace AdventOfCode.Day15;

public class Day15Puzzle
{
    public static int GetNumberOfPositionsThatCannotContainABeacon(AllMeasurements measurements, int rowNumber)
    {
        return 0;
    }
}

public record AllMeasurements(Measurement[] Measurements);

public record Measurement(Sensor sensor, Beacon beacon);

public record Beacon(Coordinate Coordinate);

public record Sensor(Coordinate SensorCoordinate);

public record Coordinate(int X, int Y);