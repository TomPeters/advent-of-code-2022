using System.Diagnostics.Metrics;

namespace AdventOfCode.Day15;

public class Day15Puzzle
{
    public static int GetNumberOfPositionsThatCannotContainABeacon(AllMeasurements allMeasurements, int rowNumber)
    {
        return allMeasurements.GetCoordinatesThatCantBeABeaconInRow(rowNumber).Count();
    }

    public static long GetTuningFrequencyOfSingleUndetectedCoordinateInRange(AllMeasurements allMeasurements, int minCoordinateValue,
        int maxCoordinateValue)
    {
        checked
        {
            var coordinate = allMeasurements.GetSingleCoordinateInRangeOutsideOfSensedRegions(minCoordinateValue,
                    maxCoordinateValue);
            return coordinate.X * 4000000L + coordinate.Y;
        }
    }
}

public record AllMeasurements(Measurement[] Measurements)
{
    public IEnumerable<Coordinate> GetCoordinatesThatCantBeABeaconInRow(int rowNumber)
    {
        var allXCoords = Measurements.SelectMany(m => new[] { m.Beacon.Coordinate, m.Sensor.Coordinate })
            .Select(c => c.X).ToArray();
        var minX = allXCoords.Min() - 1;
        var maxX = allXCoords.Max() + 1;
        var dx = maxX - minX;
        var centrePoint = (maxX - minX) / 2;
        var xRange = Enumerable.Range(centrePoint - dx, dx * 2);
        var possibleCoordinatesToConsider = xRange.Select(x => new Coordinate(x, rowNumber));

        var betweenASensorAndABeacon = possibleCoordinatesToConsider.Where(coordinate =>
        {
            var cantBeABeacon = Measurements.Any(m => m.IsInSensedRegion(coordinate));
            return cantBeABeacon;
        });

        var allCoordinatesThatCantBeABeacon = betweenASensorAndABeacon.Except(Measurements.Select(m => m.Beacon.Coordinate));
        return allCoordinatesThatCantBeABeacon;
    }
    
    public Coordinate GetSingleCoordinateInRangeOutsideOfSensedRegions(int minCoordinateValue, int maxCoordinateValue)
    {
        var candidateCoordinatesInRange = Measurements.SelectMany(m => m.GetSurroundingCoordinatesOfSensedRegion())
            .Distinct()
            .Where(c => c.X >= minCoordinateValue)
            .Where(c => c.Y >= minCoordinateValue)
            .Where(c => c.X <= maxCoordinateValue)
            .Where(c => c.Y <= maxCoordinateValue).ToArray();
        var coordinatesNotSensedByAnySensor = candidateCoordinatesInRange
            .Where(c =>
        {
            return !Measurements.Any(m => m.IsInSensedRegion(c));
        });
        return coordinatesNotSensedByAnySensor.Single();
    }
}

public class Measurement
{
    public Measurement(Sensor sensor, Beacon beacon)
    {
        this.Sensor = sensor;
        this.Beacon = beacon;
    }

    public bool IsInSensedRegion(Coordinate coordinate)
    {
        return coordinate.GetManhattanDistanceTo(Sensor.Coordinate) <=
               Beacon.Coordinate.GetManhattanDistanceTo(Sensor.Coordinate);
    }

    public Sensor Sensor { get; }
    public Beacon Beacon { get; }
    
    public IEnumerable<Coordinate> GetSurroundingCoordinatesOfSensedRegion()
    {
        var distanceToBeacon = Sensor.Coordinate.GetManhattanDistanceTo(Beacon.Coordinate);
        var distanceToSurroundingPoints = distanceToBeacon + 1;
        var start = Sensor.Coordinate with { Y = Sensor.Coordinate.Y - distanceToSurroundingPoints };
        var coordinatesAlongRightEdge = CoordinatesAlongRightEdgeOfSensedRegion(start, distanceToSurroundingPoints).ToArray();
        var coordinatesAlongLeftEdge = CoordinatesAlongLeftEdgeOfSensedRegion(start, distanceToSurroundingPoints).ToArray();
        return coordinatesAlongLeftEdge.Union(coordinatesAlongRightEdge);
    }
    
    IEnumerable<Coordinate> CoordinatesAlongRightEdgeOfSensedRegion(Coordinate topCoordinateOfRegion, int radiusOfRegion)
    {
        return CoordinatesAlongSideEdgeOfSensedRegion(topCoordinateOfRegion, radiusOfRegion, i => i);
    }
    
    IEnumerable<Coordinate> CoordinatesAlongLeftEdgeOfSensedRegion(Coordinate topCoordinateOfRegion, int radiusOfRegion)
    {
        return CoordinatesAlongSideEdgeOfSensedRegion(topCoordinateOfRegion, radiusOfRegion, i => -i);
    }

    IEnumerable<Coordinate> CoordinatesAlongSideEdgeOfSensedRegion(Coordinate topCoordinateOfRegion, int radiusOfRegion, Func<int, int> getXOffsetFromCentreLie)
    {
        return Enumerable.Range(topCoordinateOfRegion.Y, radiusOfRegion * 2 + 1).Select((y, i) =>
        {
            if (i < radiusOfRegion + 1)
            {
                var offset = getXOffsetFromCentreLie(i);
                var x = topCoordinateOfRegion.X + offset;
                return new Coordinate(x, y);
            }
            else
            {
                var offset = getXOffsetFromCentreLie(radiusOfRegion * 2 - i);
                var x = topCoordinateOfRegion.X + offset;
                return new Coordinate(x, y);
            }
        });
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