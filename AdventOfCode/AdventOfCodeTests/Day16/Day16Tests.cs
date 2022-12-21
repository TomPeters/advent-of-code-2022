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
        Assert.Equal(1651, Day16Puzzle.GetTheMostPressureThatCanBeReleased(input, 30));
    }
    
    [Fact]
    public void Part1_WorksForRealData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day16", "RealData.txt"));
        Assert.Equal(1647, Day16Puzzle.GetTheMostPressureThatCanBeReleased(input, 30));
    }
    
    [Fact]
    public void Part2_WorksForSampleData()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day16", "Sample.txt"));
        Assert.Equal(1707, Day16Puzzle.GetTheMostPressureThatCanBeReleasedByTwoActors(input, 26));
    }

    [Fact]
    public void SequenceOutputsCorrectTotalPressure()
    {
        var input = ParseInput(FileHelper.ReadFromFile("Day16", "Sample.txt"));
        var fullNetwork = CompleteNetwork.CreateNetwork(input);
        var network = SimplifiedNetwork.Create(fullNetwork);
        var startingRoom = network.GetRoom("AA");
        var roomsWithClosedValves = new HashSet<Room>(network.RoomsWithValvesToBeOpened);

        var startingSequence =
            new CandidateSequenceFor2Actors(startingRoom, Enumerable.Empty<IOperation>(), roomsWithClosedValves);

        var first = Actor1MovesToValveII(startingSequence);
        var second = Actor2MoveToValveDD(first);
        var third = Actor2OpensValveDD(second);
        var fourth = Actor1OpensValveJJ(third);
        var fifth = Actor2MoveToValveHH(fourth);
        var sixth = Actor1MoveToValveBB(fifth);
        var seventh = Actor1OpensValveBB(sixth);
        var eighth = Actor2OpensValveHH(seventh);
        var ninth = Actor1MoveToValveCC(eighth);
        var tenth = Actor2MoveToValveEE(ninth);
        var eleventh = Actor1OpensValveCC(tenth);
        var twelfth = Actor1MoveToValveEE(eleventh);
        var final = Actor2OpensValveEE(twelfth);
        
        Assert.Empty(final.GetPossibleNextOperations(15));

        Assert.Equal(1707, final.GetTotalPressureReleased());

        CandidateSequenceFor2Actors Actor1MovesToValveII(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(26).Where(o => o is MoveToRoomOperation moveToRoomOperation)
                .Cast<MoveToRoomOperation>().Single(r => r.NewRoom.Valve.Name == "JJ");

            Assert.Equal(Actor.Actor1, operation.Actor);
            Assert.Equal(24, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }

        CandidateSequenceFor2Actors Actor2MoveToValveDD(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(26).Where(o => o is MoveToRoomOperation moveToRoomOperation)
                .Cast<MoveToRoomOperation>().Single(r => r.NewRoom.Valve.Name == "DD");
        
            Assert.Equal(Actor.Actor2, operation.Actor);
            Assert.Equal(25, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor2OpensValveDD(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(25).Where(o => o is TurnOnValveInRoomOperation turnOnValveInRoomOperation)
                .Cast<TurnOnValveInRoomOperation>().Single(r => r.Room.Valve.Name == "DD");

            Assert.Equal(Actor.Actor2, operation.Actor);
            Assert.Equal(24, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor1OpensValveJJ(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(24).Where(o => o is TurnOnValveInRoomOperation turnOnValveInRoomOperation)
                .Cast<TurnOnValveInRoomOperation>().Single(r => r.Room.Valve.Name == "JJ");

            Assert.Equal(Actor.Actor1, operation.Actor);
            Assert.Equal(23, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor2MoveToValveHH(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(24).Where(o => o is MoveToRoomOperation moveToRoomOperation)
                .Cast<MoveToRoomOperation>().Single(r => r.NewRoom.Valve.Name == "HH");
        
            Assert.Equal(Actor.Actor2, operation.Actor);
            Assert.Equal(20, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor1MoveToValveBB(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(23).Where(o => o is MoveToRoomOperation moveToRoomOperation)
                .Cast<MoveToRoomOperation>().Single(r => r.NewRoom.Valve.Name == "BB");
        
            Assert.Equal(Actor.Actor1, operation.Actor);
            Assert.Equal(20, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor1OpensValveBB(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(20).Where(o => o is TurnOnValveInRoomOperation turnOnValveInRoomOperation)
                .Cast<TurnOnValveInRoomOperation>().Single(r => r.Room.Valve.Name == "BB");

            Assert.Equal(Actor.Actor1, operation.Actor);
            Assert.Equal(19, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor2OpensValveHH(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(20).Where(o => o is TurnOnValveInRoomOperation turnOnValveInRoomOperation)
                .Cast<TurnOnValveInRoomOperation>().Single(r => r.Room.Valve.Name == "HH");

            Assert.Equal(Actor.Actor2, operation.Actor);
            Assert.Equal(19, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor1MoveToValveCC(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(19).Where(o => o is MoveToRoomOperation moveToRoomOperation)
                .Cast<MoveToRoomOperation>().Single(r => r.NewRoom.Valve.Name == "CC");
        
            Assert.Equal(Actor.Actor1, operation.Actor);
            Assert.Equal(18, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor2MoveToValveEE(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(19).Where(o => o is MoveToRoomOperation moveToRoomOperation)
                .Cast<MoveToRoomOperation>().Single(r => r.NewRoom.Valve.Name == "EE");
        
            Assert.Equal(Actor.Actor2, operation.Actor);
            Assert.Equal(16, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor1OpensValveCC(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(18).Where(o => o is TurnOnValveInRoomOperation turnOnValveInRoomOperation)
                .Cast<TurnOnValveInRoomOperation>().Single(r => r.Room.Valve.Name == "CC");

            Assert.Equal(Actor.Actor1, operation.Actor);
            Assert.Equal(17, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
        
        CandidateSequenceFor2Actors Actor1MoveToValveEE(CandidateSequenceFor2Actors sequence)
        {
            var operation = sequence
                .GetPossibleNextOperations(17).Where(o => o is MoveToRoomOperation moveToRoomOperation)
                .Cast<MoveToRoomOperation>().Single(r => r.NewRoom.Valve.Name == "EE");
        
            Assert.Equal(Actor.Actor1, operation.Actor);
            Assert.Equal(15, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }

        CandidateSequenceFor2Actors Actor2OpensValveEE(CandidateSequenceFor2Actors sequence)
        {
            var possibleNextOperations = sequence
                .GetPossibleNextOperations(16).ToArray();
            var operation = possibleNextOperations.Where(o => o is TurnOnValveInRoomOperation turnOnValveInRoomOperation)
                .Cast<TurnOnValveInRoomOperation>().Single(r => r.Room.Valve.Name == "EE");

            Assert.Equal(Actor.Actor2, operation.Actor);
            Assert.Equal(15, operation.TimeForActorsNextAction());

            return sequence.AddOperation(operation);
        }
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