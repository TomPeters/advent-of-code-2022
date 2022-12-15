using System.Linq;
using AdventOfCode.Day15;
using Xunit;

namespace AdventOfCodeTests.Day15;

public class Day15Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day15", "Sample.txt"));
        Assert.Equal(26, Day15Puzzle.GetNumberOfPositionsThatCannotContainABeacon(input, 10));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day15", "RealData.txt"));
        Assert.Equal(5508234, Day15Puzzle.GetNumberOfPositionsThatCannotContainABeacon(input, 2000000));
    }

    private AllMeasurements ParseInput(string input)
    {
        var measurements = input.Split("\n").Select(measurementInput =>
        {
            var parts = measurementInput.Split(":");
            var sensorPart = parts[0];
            var beaconPart = parts[1];
            var sensorCoordinate = GetCoordinates(sensorPart);
            var beaconCoordinate = GetCoordinates(beaconPart);
            return new Measurement(new Sensor(sensorCoordinate), new Beacon(beaconCoordinate));
        }).ToArray();
        return new AllMeasurements(measurements);
    }

    Coordinate GetCoordinates(string inputWithCoordinates)
    {
        var startOfCoordinatePart = inputWithCoordinates.IndexOf("x=");
        var coordinateString = inputWithCoordinates.Substring(startOfCoordinatePart);
        var coordinateParts = coordinateString.Split(",");
        var xString = coordinateParts[0];
        var yString = coordinateParts[1];
        var x = int.Parse(xString.Split("=")[1]);
        var y = int.Parse(yString.Split("=")[1]);
        return new Coordinate(x, y);
    }
}