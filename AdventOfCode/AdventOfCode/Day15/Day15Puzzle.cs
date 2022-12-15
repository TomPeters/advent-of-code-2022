namespace AdventOfCode.Day15;

public class Day15Puzzle
{
    public static int GetNumberOfPositionsThatCannotContainABeacon(AllMeasurements allMeasurements, int rowNumber)
    {
        var allXCoords = allMeasurements.Measurements.SelectMany(m => new[] { m.Beacon.Coordinate, m.Sensor.Coordinate })
            .Select(c => c.X).ToArray();
        var minX = allXCoords.Min() - 1;
        var maxX = allXCoords.Max() + 1;
        var dx = maxX - minX;
        var centrePoint = (maxX - minX) / 2;
        var xRange = Enumerable.Range(centrePoint - dx, dx * 2);
        var possibleCoordinatesToConsider = xRange.Select(x => new Coordinate(x, rowNumber));

        var betweenASensorAndABeacon = possibleCoordinatesToConsider.Where(coordinate =>
        {
            var cantBeABeacon = allMeasurements.Measurements.Any(m => m.IsCoordinateBetweenSensorAndBeacon(coordinate));
            return cantBeABeacon;
        });

        var allCoordinatesThatCantBeABeacon = betweenASensorAndABeacon.Except(allMeasurements.Measurements.Select(m => m.Beacon.Coordinate));

        return allCoordinatesThatCantBeABeacon.Count();
    }

    // Assume start x and start Y are outside the range of any sensers
    static IEnumerable<Coordinate> GetCoordinatesWithinRangeOfSensor(int startX, int endX, int row)
    {
        var currentX = startX;
        while (currentX <= endX)
        {
            if ()
        }
    }
}

public record AllMeasurements(Measurement[] Measurements);

public class Measurement
{
    private IEnumerable<PairsOfCoordinatesOnTheSameRow> _knownRegionWhereThereCantBeAnyMoreBeacons;

    public Measurement(Sensor sensor, Beacon beacon)
    {
        this.Sensor = sensor;
        this.Beacon = beacon;
        _knownRegionWhereThereCantBeAnyMoreBeacons = GetKnownRegionWhereThereCantBeAnyMoreBeacons();
    }

    public IEnumerable<Coordinate> GetCoordinatesWhereThereCantBeABeacon()
    {
        var distanceToBeacon = Sensor.Coordinate.GetManhattanDistanceTo(Beacon.Coordinate);
        var xRange = Enumerable.Range(Sensor.Coordinate.X - distanceToBeacon, distanceToBeacon * 2 + 1);
        var yRange = Enumerable.Range(Sensor.Coordinate.Y - distanceToBeacon, distanceToBeacon * 2 + 1);
        var surroundingCoordinates = xRange.SelectMany(x => yRange.Select((y) => new Coordinate(x, y)));
        return surroundingCoordinates.Where(c => Sensor.Coordinate.GetManhattanDistanceTo(c) <= distanceToBeacon);
    }

    public bool IsCoordinateBetweenSensorAndBeacon(Coordinate coordinate)
    {
        return coordinate.GetManhattanDistanceTo(Sensor.Coordinate) <=
               Beacon.Coordinate.GetManhattanDistanceTo(Sensor.Coordinate);
    }

    public Sensor Sensor { get; }
    public Beacon Beacon { get; }
    
    IEnumerable<PairsOfCoordinatesOnTheSameRow> GetKnownRegionWhereThereCantBeAnyMoreBeacons()
    {
        var distanceToBeacon = Sensor.Coordinate.GetManhattanDistanceTo(Beacon.Coordinate);
        var start = Sensor.Coordinate with { Y = Sensor.Coordinate.Y - distanceToBeacon };
        var coordinatesAlongRightEdge = CoordinatesAlongRightEdgeOfSensedRegion(start, distanceToBeacon);
        var coordinatesAlongLeftEdge = CoordinatesAlongLeftEdgeOfSensedRegion(start, distanceToBeacon);
        return coordinatesAlongRightEdge.Zip(coordinatesAlongLeftEdge, (c1, c2) => new PairsOfCoordinatesOnTheSameRow(c1, c2));
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
            if (y < radiusOfRegion + 1)
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

public class AllPairsOfCoordinates
{
    private Dictionary<Coordinate, Coordinate> _coordinateLookup;

    public AllPairsOfCoordinates(IEnumerable<PairsOfCoordinatesOnTheSameRow> allCoordinatePairs)
    {
        _coordinateLookup = allCoordinatePairs.ToDictionary(c => c.FirstCoordinate, c => c.LastCoordinate);
    }
}

public record PairsOfCoordinatesOnTheSameRow(Coordinate FirstCoordinate, Coordinate LastCoordinate);

public static class ManhattanDistance
{
    public static int GetManhattanDistanceTo(this Coordinate coordinate, Coordinate otherCoordinate) => 
        Math.Abs(coordinate.X - otherCoordinate.X) + Math.Abs(coordinate.Y - otherCoordinate.Y);
}

public record Beacon(Coordinate Coordinate);

public record Sensor(Coordinate Coordinate);

public record Coordinate(int X, int Y);