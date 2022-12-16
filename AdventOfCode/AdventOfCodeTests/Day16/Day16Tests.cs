using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Day16;
using Xunit;

namespace AdventOfCodeTests.Day16;

public class Day16Tests
{
    [Fact]
    public void Part1_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day16", "Sample.txt"));
        Assert.Equal(1651, Day16Puzzle.GetTheMostPressureThatCanBeReleased(input));
    }

    private ScannedOutput ParseInput(string input)
    {
        var scannedRooms = input.Split("\n").Select(roomInput =>
        {
            var parts = roomInput.Split(";");
            var connectedRoomsInput = parts[1];
            var valveInputParts = parts[0].Split(" has flow rate=");
            var valveNameInput = valveInputParts[0];
            var flowRate = int.Parse(valveInputParts[1]);
            var valveName = valveNameInput.Split(" ")[1];
            return new ScannedRoom(valveName, flowRate, GetConnectedRooms(connectedRoomsInput));
        }).ToArray();
        return new ScannedOutput(scannedRooms);
    }

    static string[] GetConnectedRooms(string connectedRoomsInput)
    {
        return connectedRoomsInput.Split(" ").Skip(5).Select(s => s.Replace(",", "")).ToArray();
    }
}