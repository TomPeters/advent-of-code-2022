using AdventOfCode.Day15;
using FluentAssertions;
using Xunit;

namespace AdventOfCodeTests.Day15;

public class MeasurementTests
{
    [Fact]
    public void GetSurroundingCoordinatesOfSensedRegion_BeaconNextToSensorLocation_ReturnsCorrectLocations()
    {
        var measurement = new Measurement(new Sensor(new Coordinate(0, 0)), new Beacon(new Coordinate(1, 0)));
        var coordinates = measurement.GetSurroundingCoordinatesOfSensedRegion();
        coordinates.Should().BeEquivalentTo(new[]
        {
            new Coordinate(0, 2),
            new Coordinate(1, 1),
            new Coordinate(2, 0),
            new Coordinate(1, -1),
            new Coordinate(0, -2),
            new Coordinate(-1, -1),
            new Coordinate(-2, 0),
            new Coordinate(-1, 1)
        });
    }
    
    [Fact]
    public void GetSurroundingCoordinatesOfSensedRegion_Beacon2AwayFromSensorLocation_ReturnsCorrectLocations()
    {
        var measurement = new Measurement(new Sensor(new Coordinate(0, 0)), new Beacon(new Coordinate(-1, 1)));
        var coordinates = measurement.GetSurroundingCoordinatesOfSensedRegion();
        coordinates.Should().BeEquivalentTo(new[]
        {
            new Coordinate(0, 3),
            new Coordinate(1, 2),
            new Coordinate(2, 1),
            new Coordinate(3, 0),
            new Coordinate(2, -1),
            new Coordinate(1, -2),
            new Coordinate(0, -3),
            new Coordinate(-1, -2),
            new Coordinate(-2, -1),
            new Coordinate(-3, 0),
            new Coordinate(-2, 1),
            new Coordinate(-1, 2)
        });
    }
}