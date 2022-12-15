namespace AdventOfCode.Day15;

public class Day15Puzzle
{
    public static int GetNumberOfPositionsThatCannotContainABeacon(AllMeasurements allMeasurements, int rowNumber)
    {
        var allBeaconCoordinates = allMeasurements.Measurements.Select(m => m.beacon.Coordinate).Distinct();
        var allLocationsWhereThereCantBeABeacon = allMeasurements.Measurements
            .SelectMany(m => m.GetCoordinatesWhereThereCantBeABeacon()).Except(allBeaconCoordinates);
        return allLocationsWhereThereCantBeABeacon.Count(l => l.Y == rowNumber);
    }
}

public record AllMeasurements(Measurement[] Measurements);

public record Measurement(Sensor sensor, Beacon beacon)
{
    public IEnumerable<Coordinate> GetCoordinatesWhereThereCantBeABeacon()
    {
        var distanceToBeacon = sensor.Coordinate.GetManhattanDistanceTo(beacon.Coordinate);
        var xRange = Enumerable.Range(sensor.Coordinate.X - distanceToBeacon, distanceToBeacon * 2 + 1);
        var yRange = Enumerable.Range(sensor.Coordinate.Y - distanceToBeacon, distanceToBeacon * 2 + 1);
        var surroundingCoordinates = xRange.SelectMany(x => yRange.Select((y) => new Coordinate(x, y)));
        return surroundingCoordinates.Where(c => sensor.Coordinate.GetManhattanDistanceTo(c) <= distanceToBeacon);
    }
}

public static class ManhattanDistance
{
    public static int GetManhattanDistanceTo(this Coordinate coordinate, Coordinate otherCoordinate) => 
        Math.Abs(coordinate.X - otherCoordinate.X) + Math.Abs(coordinate.Y - otherCoordinate.Y);
}

public record Beacon(Coordinate Coordinate);

public record Sensor(Coordinate Coordinate);

public record Coordinate(int X, int Y);