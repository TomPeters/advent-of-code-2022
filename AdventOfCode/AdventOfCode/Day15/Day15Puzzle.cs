namespace AdventOfCode.Day15;

public class Day15Puzzle
{
    public static int GetNumberOfPositionsThatCannotContainABeacon(AllMeasurements allMeasurements, int rowNumber)
    {
        var allXCoords = allMeasurements.Measurements.SelectMany(m => new[] { m.beacon.Coordinate, m.sensor.Coordinate })
            .Select(c => c.X).ToArray();
        var minX = allXCoords.Min();
        var maxX = allXCoords.Max();
        var dx = maxX - minX;
        var centrePoint = (maxX - minX) / 2;
        var xRange = Enumerable.Range(centrePoint - dx, dx * 2);
        var possibleCoordinatesToConsider = xRange.Select(x => new Coordinate(x, rowNumber));

        var betweenASensorAndABeacon = possibleCoordinatesToConsider.Where(coordinate =>
        {
            var cantBeABeacon = allMeasurements.Measurements.Any(m => m.IsCoordinateBetweenSensorAndBeacon(coordinate));
            return cantBeABeacon;
        });

        var allCoordinatesThatCantBeABeacon = betweenASensorAndABeacon.Except(allMeasurements.Measurements.Select(m => m.beacon.Coordinate));

        return allCoordinatesThatCantBeABeacon.Count();
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

    public bool IsCoordinateBetweenSensorAndBeacon(Coordinate coordinate)
    {
        return coordinate.GetManhattanDistanceTo(sensor.Coordinate) <=
               beacon.Coordinate.GetManhattanDistanceTo(sensor.Coordinate);
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