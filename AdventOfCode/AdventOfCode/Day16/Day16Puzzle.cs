using System.Diagnostics;

namespace AdventOfCode.Day16;

public class Day16Puzzle
{
    public static int GetTheMostPressureThatCanBeReleased(ScannedOutput scannedOutput, int timeToComplete)
    {
        var fullNetwork = CompleteNetwork.CreateNetwork(scannedOutput);
        var simplifiedNetwork = SimplifiedNetwork.Create(fullNetwork);
        var allCandidateSequences = simplifiedNetwork.GetAllCandidateSequences(timeToComplete);
        return allCandidateSequences.Max(s => s.GetTotalPressureReleased());
    }
    
    public static int GetTheMostPressureThatCanBeReleasedByTwoActors(ScannedOutput scannedOutput, int timeToComplete)
    {
        var fullNetwork = CompleteNetwork.CreateNetwork(scannedOutput);
        var simplifiedNetwork = SimplifiedNetwork.Create(fullNetwork);
        var allCandidateSequences = simplifiedNetwork.GetAllCandidateSequencesForTwoConcurrentActors(timeToComplete);
        var candidateSequenceFor2Actors = allCandidateSequences.MaxBy(s => s.GetTotalPressureReleased());
        return candidateSequenceFor2Actors.GetTotalPressureReleased();
    }
}

public enum Actor
{
    Actor1,
    Actor2
}

public interface IOperation
{
    Actor Actor { get; }
    int GetPressureReleased();
    Room? GetNewRoom();
    Room? GetRoomWithValveOpened();
    int TimeForActorsNextAction();
    int TimeRequiredToExecute();
}

public class MoveToRoomOperation : IOperation
{
    private readonly int _startingTime;
    public int TravelTime { get; }
    public Room NewRoom { get; }

    public MoveToRoomOperation(int startingTime, Room originalRoom, Room newRoom, Actor actor)
    {
        _startingTime = startingTime;
        TravelTime = originalRoom.GetTravelTimeTo(newRoom);
        NewRoom = newRoom;
        Actor = actor;
    }

    public Actor Actor { get; }
    public int GetPressureReleased() => 0;
    public Room? GetNewRoom()
    {
        return NewRoom;
    }

    public Room? GetRoomWithValveOpened() => null;
    public int TimeForActorsNextAction() => _startingTime - TimeRequiredToExecute();
    public int TimeRequiredToExecute() => TravelTime;
}

public class TurnOnValveInRoomOperation : IOperation
{
    public Room Room { get; }
    private readonly int _startingTime;

    public TurnOnValveInRoomOperation(Room room, int startingTime, Actor actor)
    {
        Room = room;
        _startingTime = startingTime;
        Actor = actor;
    }

    public Actor Actor { get; }
    public int GetPressureReleased() => (_startingTime - 1) * Room.Valve.FlowRate;
    public Room? GetNewRoom() => null;

    public Room? GetRoomWithValveOpened() => Room;
    public int TimeForActorsNextAction() => _startingTime - TimeRequiredToExecute();
    public int TimeRequiredToExecute() => 1;
}

public class SimplifiedNetwork
{
    private readonly Room[] _allRooms;

    private SimplifiedNetwork(IEnumerable<Room> rooms)
    {
        _allRooms = rooms.ToArray();
    }

    public static SimplifiedNetwork Create(CompleteNetwork completeNetwork)
    {
        var startingRoom = completeNetwork.Rooms.Single(r => r.Valve.Name == "AA");
        var roomsInSimplifiedNetwork = completeNetwork.Rooms.Where(r => r.Valve.HasAnyPressure()).Concat(new [] { startingRoom }).ToArray();
        var rooms = roomsInSimplifiedNetwork.Select(Room.CreateFrom).ToDictionary(r => r.Valve);
        var roomPairs = roomsInSimplifiedNetwork.SelectMany(r1 =>
            roomsInSimplifiedNetwork.Where(r2 => r2 != r1).Select(r2 => (r1, r2)));
        roomPairs.ForEach(pair =>
        {
            var (originalRoom1, originalRoom2) = pair;
            var room1 = rooms[originalRoom1.Valve];
            var room2 = rooms[originalRoom2.Valve];
            var travelTime = GetShortestTravelTimeBetweenRooms(originalRoom1, originalRoom2);
            Room.Connect(room1, room2, travelTime);
        });
        return new SimplifiedNetwork(rooms.Values);
    }

    private static int GetShortestTravelTimeBetweenRooms(OriginalRoom originalRoom1, OriginalRoom originalRoom2)
    {
        var shortestPathsToAllRooms = GetShortestPathsToAllRooms(originalRoom1);
        if (!shortestPathsToAllRooms.ContainsKey(originalRoom2))
        {
            throw new Exception("No path to room");
        }

        return shortestPathsToAllRooms[originalRoom2];
    }

    private static IDictionary<OriginalRoom, int> GetShortestPathsToAllRooms(OriginalRoom startingRoom)
    {
        var shortestPaths = new Dictionary<OriginalRoom, int>();
        var roomsToCheck = new List<OriginalRoom>(startingRoom.GetConnectedRooms());
        var currentTravelTime = 1;
        while (roomsToCheck.Any())
        {
            var nextRoomsToCheck = new List<OriginalRoom>();
            foreach (var roomToCheck in roomsToCheck)
            {
                if (!shortestPaths.ContainsKey(roomToCheck))
                {
                    shortestPaths[roomToCheck] = currentTravelTime;
                    nextRoomsToCheck.AddRange(roomToCheck.GetConnectedRooms());
                }
            }

            roomsToCheck = nextRoomsToCheck;
            currentTravelTime++;
        }

        return shortestPaths;
    }
    
    public IEnumerable<CandidateSequenceFor2Actors> GetAllCandidateSequencesForTwoConcurrentActors(int timeToComplete)
    {
        var startingRoom = _allRooms.Single(r => r.Valve.Name == "AA");
        
        // All rooms that have flow rates with zero have already been removed from the network, with the one exception of the starting room
        // To avoid opening the valve in this starting room, we exclude it from this collection
        var roomsWithValvesToBeOpened = _allRooms.Except(new [] {startingRoom});
        return GetAllCandidateSequencesFor2Actors(timeToComplete,
            new CandidateSequenceFor2Actors(startingRoom, Array.Empty<IOperation>(), new HashSet<Room>(roomsWithValvesToBeOpened)));
    }

    IEnumerable<CandidateSequenceFor2Actors> GetAllCandidateSequencesFor2Actors(int timeRemaining, CandidateSequenceFor2Actors sequenceSoFar)
    {
        var possibleOperations = sequenceSoFar.GetPossibleNextOperations(timeRemaining).Where(o => o.TimeRequiredToExecute() <= timeRemaining).ToArray();

        if (timeRemaining == 0 || !possibleOperations.Any())
        {
            yield return sequenceSoFar;
            yield break;
        }
        foreach (var operation in possibleOperations)
        {
            var sequenceWithNextStep = sequenceSoFar.AddOperation(operation);
            foreach (var nextSequence
                     in GetAllCandidateSequencesFor2Actors(sequenceSoFar.GetTimeForNextActor() ?? timeRemaining, sequenceWithNextStep))
            {
                yield return nextSequence;
            }
        }
    }

    public IEnumerable<CandidateSequence> GetAllCandidateSequences(int timeToComplete)
    {
        var startingRoom = _allRooms.Single(r => r.Valve.Name == "AA");
        
        // All rooms that have flow rates with zero have already been removed from the network, with the one exception of the starting room
        // To avoid opening the valve in this starting room, we exclude it from this collection
        var roomsWithValvesToBeOpened = _allRooms.Except(new [] {startingRoom});
        return GetAllCandidateSequences(timeToComplete,
            new CandidateSequence(startingRoom, Array.Empty<IOperation>(), new HashSet<Room>(roomsWithValvesToBeOpened)));
    }

    IEnumerable<CandidateSequence> GetAllCandidateSequences(int timeRemaining, CandidateSequence sequenceSoFar)
    {
        if (timeRemaining < 0)
        {
            Debugger.Break();
        }
        var possibleOperations = sequenceSoFar.GetPossibleNextOperations(timeRemaining).Where(o => o.TimeRequiredToExecute() <= timeRemaining).ToArray();

        if (timeRemaining == 0 || !possibleOperations.Any())
        {
            yield return sequenceSoFar;
            yield break;
        }
        foreach (var operation in possibleOperations)
        {
            var sequenceWithNextStep = sequenceSoFar.AddOperation(operation);
            foreach (var nextSequence
                     in GetAllCandidateSequences(operation.TimeForActorsNextAction(), sequenceWithNextStep))
            {
                yield return nextSequence;
            }
        }
    }
}

public class CandidateSequence
{
    private readonly HashSet<Room> _roomsWithClosedValves;
    public Room LastRoom { get; }
    private readonly IOperation[] _operations;

    public CandidateSequence(Room lastRoom, IEnumerable<IOperation> operations, HashSet<Room> roomsWithClosedValves)
    {
        _roomsWithClosedValves = roomsWithClosedValves;
        LastRoom = lastRoom;
        _operations = operations.ToArray();
    }

    public CandidateSequence AddOperation(IOperation newOperation)
    {
        var operations = _operations.Concat(new[] { newOperation });
        var nextRoom = newOperation.GetNewRoom() ?? LastRoom;
        var roomWithOpenedValve = newOperation.GetRoomWithValveOpened();
        var roomsWithClosedValves = roomWithOpenedValve == null
            ? _roomsWithClosedValves
            : new HashSet<Room>(_roomsWithClosedValves.Except(new[] { roomWithOpenedValve }));
        return new CandidateSequence(nextRoom, operations, roomsWithClosedValves);
    }

    public int GetTotalPressureReleased() => _operations.Sum(o => o.GetPressureReleased());

    public IEnumerable<IOperation> GetPossibleNextOperations(int timeRemaining)
    {
        var currentRoom = LastRoom;

        var lastOperationWasAMove = _operations.LastOrDefault()?.GetNewRoom() != null; // first room => false
        if (_roomsWithClosedValves.Contains(currentRoom))
        {
            yield return new TurnOnValveInRoomOperation(currentRoom, timeRemaining, Actor.Actor1);
        }

        // Because the network is completely connected, we never want to perform two moves in a row
        if (!lastOperationWasAMove)
        {
            var otherRoomsWithClosedValves = _roomsWithClosedValves.Except(new[] { currentRoom })
                .Select(r => new MoveToRoomOperation(timeRemaining, currentRoom, r, Actor.Actor1));
            foreach (var moveToRoomOperation in otherRoomsWithClosedValves)
            {
                yield return moveToRoomOperation;
            }
        }
    }
}

public class CandidateSequenceFor2Actors
{
    private readonly Room _startingRoom;
    private readonly HashSet<Room> _roomsWithClosedValves;
    private readonly IOperation[] _operations;

    public CandidateSequenceFor2Actors(Room startingRoom, IEnumerable<IOperation> operations, HashSet<Room> roomsWithClosedValves)
    {
        _startingRoom = startingRoom;
        _roomsWithClosedValves = roomsWithClosedValves;
        _operations = operations.ToArray();
    }

    public CandidateSequenceFor2Actors AddOperation(IOperation newOperation)
    {
        var operations = _operations.Concat(new[] { newOperation });
        var roomWithOpenedValve = newOperation.GetRoomWithValveOpened();
        var roomsWithClosedValves = roomWithOpenedValve == null
            ? _roomsWithClosedValves
            : new HashSet<Room>(_roomsWithClosedValves.Except(new[] { roomWithOpenedValve }));
        return new CandidateSequenceFor2Actors(_startingRoom, operations, roomsWithClosedValves);
    }

    public int? GetTimeForNextActor()
    {
        return new[] { Actor.Actor1, Actor.Actor2 }.Select(a => _operations.LastOrDefault(o => o.Actor == a)?.TimeForActorsNextAction()).Max();
    }

    public int GetTotalPressureReleased() => _operations.Sum(o => o.GetPressureReleased());

    public IEnumerable<IOperation> GetPossibleNextOperations(int timeRemaining)
    {
        var currentActor = !_operations.Any() ? Actor.Actor1 :
            new[] { Actor.Actor1, Actor.Actor2 }.Select(a => (a, _operations.LastOrDefault(o => o.Actor == a)?.TimeForActorsNextAction() ?? timeRemaining)).MaxBy(t => t.Item2).a;

        var operationsForCurrentActors = _operations.Where(o => o.Actor == currentActor).ToArray();
        
        var actorsCurrentRoom = operationsForCurrentActors.Select(r => r.GetNewRoom()).LastOrDefault(r => r != null) ??
                                _startingRoom;
        
        var lastOperationWasAMove = operationsForCurrentActors.LastOrDefault()?.GetNewRoom() != null; // first room => false
        if (_roomsWithClosedValves.Contains(actorsCurrentRoom))
        {
            yield return new TurnOnValveInRoomOperation(actorsCurrentRoom, timeRemaining, currentActor);
        }

        // Because the network is completely connected, we never want to perform two moves in a row
        if (!lastOperationWasAMove)
        {
            var otherRoomsWithClosedValves = _roomsWithClosedValves.Except(new[] { actorsCurrentRoom })
                .Select(r => new MoveToRoomOperation(timeRemaining, actorsCurrentRoom, r, currentActor));
            foreach (var moveToRoomOperation in otherRoomsWithClosedValves)
            {
                yield return moveToRoomOperation;
            }
        }
    }
}

public class Room
{
    public Valve Valve { get; }
    private readonly Dictionary<Room, int> _connectedRooms = new();

    public static Room CreateFrom(OriginalRoom room)
    {
        return new Room(room.Valve);
    }
    Room(Valve valve)
    {
        Valve = valve;
    }

    void ConnectToRoom(Room room, int travelTime)
    {
        if (!_connectedRooms.ContainsKey(room))
        {
            _connectedRooms.Add(room, travelTime);
        }
    }

    public static void Connect(Room room1, Room room2, int travelTime)
    {
        room1.ConnectToRoom(room2, travelTime);
        room2.ConnectToRoom(room1, travelTime);
    }

    public int GetTravelTimeTo(Room room)
    {
        return _connectedRooms[room];
    }
}

public class CompleteNetwork
{
    private readonly OriginalRoom _startingRoom;
    public OriginalRoom[] Rooms { get; }

    public static CompleteNetwork CreateNetwork(ScannedOutput scannedOutput)
    {
        var allRooms = scannedOutput.ScannedRooms.Select(r => new OriginalRoom(new Valve(r.Valve, r.FlowRate)))
            .ToArray();
        var roomLookup = allRooms.ToDictionary(r => r.Valve.Name);
        var startingRoom = roomLookup["AA"];
        scannedOutput.ScannedRooms.ForEach(sr =>
        {
            sr.ConnectedValves.ForEach(roomName =>
            {
                OriginalRoom.Connect(roomLookup[sr.Valve], roomLookup[roomName]);
            });
        });
        return new CompleteNetwork(allRooms, startingRoom);
    }

    CompleteNetwork(OriginalRoom[] allRooms, OriginalRoom startingRoom)
    {
        Rooms = allRooms;
        _startingRoom = startingRoom;
    }
}

public class OriginalRoom
{
    protected bool Equals(OriginalRoom other)
    {
        return Valve.Equals(other.Valve);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((OriginalRoom)obj);
    }

    public override int GetHashCode()
    {
        return Valve.GetHashCode();
    }

    public Valve Valve { get; init; }
    private readonly ISet<OriginalRoom> _connectedRooms = new HashSet<OriginalRoom>();

    public IEnumerable<OriginalRoom> GetConnectedRooms() => _connectedRooms;

    public OriginalRoom(Valve valve)
    {
        Valve = valve;
    }

    public static void Connect(OriginalRoom firstRoom, OriginalRoom secondRoom)
    {
        firstRoom._connectedRooms.Add(secondRoom);
        secondRoom._connectedRooms.Add(firstRoom);
    }
}

public record Valve(string Name, int FlowRate)
{
    public bool HasAnyPressure() => FlowRate != 0;
}

public record ScannedOutput(ScannedRoom[] ScannedRooms);

public record ScannedRoom(string Valve, int FlowRate, string[] ConnectedValves);