namespace AdventOfCode.Day16;

public class Day16Puzzle
{
    public static int GetTheMostPressureThatCanBeReleased(ScannedOutput scannedOutput)
    {
        var network = CompleteNetwork.CreateNetwork(scannedOutput);
        var allCandidateSequences = network.GetAllCandidateSequences(30).Where(s => s.CompleteSequenceIsValid());
        return allCandidateSequences.Max(s => s.GetTotalPressureReleased());
    }
}

public class CandidateSequence
{
    private readonly AllRooms _allRooms;
    private readonly int _totalTimeToComplete;
    private readonly HashSet<string> _openedValves;
    private readonly HashSet<OriginalRoom> _roomsVisitedSinceLastValveOpened;
    public OriginalRoom LastRoom { get; }
    private readonly IOperation[] _operations;

    public CandidateSequence(AllRooms allRooms, int totalTimeToComplete, OriginalRoom lastRoom, IEnumerable<IOperation> operations, HashSet<string> openedValves, HashSet<OriginalRoom> roomsVisitedSinceLastValveOpened)
    {
        _allRooms = allRooms;
        _totalTimeToComplete = totalTimeToComplete;
        _openedValves = openedValves;
        _roomsVisitedSinceLastValveOpened = roomsVisitedSinceLastValveOpened;
        LastRoom = lastRoom;
        _operations = operations.ToArray();
    }

    public CandidateSequence AddOperation(IOperation newOperation)
    {
        var operations = _operations.Concat(new[] { newOperation });
        var newRoom = newOperation.GetNewRoom();
        var nextRoom = newRoom ?? LastRoom;
        var openedValve = newOperation.GetValveOpened();
        var openedValves = openedValve == null
            ? _openedValves
            : new HashSet<string>(_openedValves.Concat(new[] { openedValve }));
        HashSet<OriginalRoom> roomsSinceLastOpenValveOperation = openedValve != null ? new HashSet<OriginalRoom>() :
            newRoom == null ? _roomsVisitedSinceLastValveOpened : new HashSet<OriginalRoom>(_roomsVisitedSinceLastValveOpened.Concat(new [] { newRoom }));
        return new CandidateSequence(_allRooms, _totalTimeToComplete, nextRoom, operations, openedValves, roomsSinceLastOpenValveOperation);
    }

    public int GetTotalPressureReleased() => _operations.Sum(o => o.GetPressureReleased());

    public bool CanOpenValve(string valveName) => !_openedValves.Contains(valveName);

    public bool AnyValvesLeftToOpen()
    {
        return _allRooms.GetValvesThatHaveAnyPressure().Any(CanOpenValve);
    }

    public bool CompleteSequenceIsValid()
    {
        return _operations.Length == _totalTimeToComplete;
    }

    public IEnumerable<IOperation> GetPossibleNextOperations(int timeRemaining)
    {
        var currentRoom = LastRoom;
        var currentValveHasPressure = currentRoom.Valve.HasAnyPressure();
        var canTurnValveOn = currentValveHasPressure && CanOpenValve(currentRoom.Valve.Name);
        if (canTurnValveOn)
        {
            yield return new TurnOnValveInRoomOperation(currentRoom, timeRemaining - 1);
        }

        if (!AnyValvesLeftToOpen())
        {
            yield return new DoNothingOperation();
        }
        else
        {
            // Only bother moving if there are any valves left to open. This is an optimisation to reduce the solution space.
            foreach (var connectedRoom in currentRoom.GetConnectedRooms().Where(r => !_roomsVisitedSinceLastValveOpened.Contains(r)))
            {
                yield return new MoveToRoomOperation(connectedRoom);
            }
        }
    }
}

public interface IOperation
{
    int GetPressureReleased();
    OriginalRoom? GetNewRoom();
    string? GetValveOpened();
}

public class MoveToRoomOperation : IOperation
{
    public OriginalRoom NewRoom { get; }

    public MoveToRoomOperation(OriginalRoom newRoom)
    {
        NewRoom = newRoom;
    }

    public int GetPressureReleased() => 0;
    public OriginalRoom? GetNewRoom()
    {
        return NewRoom;
    }

    public string? GetValveOpened() => null;
}

public class TurnOnValveInRoomOperation : IOperation
{
    public OriginalRoom Room { get; }
    private readonly int _timeAvailableForValveToReleasePressure;

    public TurnOnValveInRoomOperation(OriginalRoom room, int timeAvailableForValveToReleasePressure)
    {
        Room = room;
        _timeAvailableForValveToReleasePressure = timeAvailableForValveToReleasePressure;
    }

    public int GetPressureReleased() => _timeAvailableForValveToReleasePressure * Room.Valve.FlowRate;
    public OriginalRoom? GetNewRoom() => null;

    public string? GetValveOpened() => Room.Valve.Name;
}

public class DoNothingOperation : IOperation
{
    public int GetPressureReleased() => 0;
    public OriginalRoom? GetNewRoom() => null;

    public string? GetValveOpened() => null;
}

public class AllRooms
{
    private readonly string[] _valvesWithAnyPressure;
    private readonly Dictionary<string, OriginalRoom> _roomsByValveName;
    private OriginalRoom[] _roomsArray;

    public AllRooms(IEnumerable<OriginalRoom> rooms)
    {
        _roomsArray = rooms.ToArray();
        _valvesWithAnyPressure = _roomsArray.Where(r => r.Valve.HasAnyPressure()).Select(r => r.Valve.Name).ToArray();
        _roomsByValveName = _roomsArray.ToDictionary(r => r.Valve.Name);
    }

    public string[] GetValvesThatHaveAnyPressure() => _valvesWithAnyPressure;

    public OriginalRoom GetRoomByValveName(string valveName) => _roomsByValveName[valveName];

    public OriginalRoom[] GetAll() => _roomsArray;
}

public class SimplifiedNetwork
{
    private SimplifiedNetwork(IEnumerable<Room> rooms)
    {
    }

    public static SimplifiedNetwork Create(CompleteNetwork completeNetwork)
    {
        var allRooms = completeNetwork.Rooms.GetAll();
        var roomsInSimplifiedNetwork = allRooms.Where(r => r.Valve.HasAnyPressure()).ToArray();
        var rooms = roomsInSimplifiedNetwork.Select(Room.CreateFrom).ToDictionary(r => r.RoomValve);
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
        // TODO: Implement
        return 1;
    }
}

public record RoomConnection(Room Room, int TravelTime);

public class Room
{
    public Valve RoomValve { get; }
    private readonly HashSet<RoomConnection> _connectedRooms = new();

    public static Room CreateFrom(OriginalRoom room)
    {
        return new Room(room.Valve);
    }
    Room(Valve roomValve)
    {
        RoomValve = roomValve;
    }

    public void ConnectToRoom(Room room, int travelTime)
    {
        _connectedRooms.Add(new RoomConnection(room, travelTime));
    }

    public static void Connect(Room room1, Room room2, int travelTime)
    {
        room1.ConnectToRoom(room2, travelTime);
        room2.ConnectToRoom(room1, travelTime);
    }
}

public class CompleteNetwork
{
    private readonly OriginalRoom _startingRoom;
    public AllRooms Rooms { get; }

    public static CompleteNetwork CreateNetwork(ScannedOutput scannedOutput)
    {
        var allRooms = new AllRooms(scannedOutput.ScannedRooms.Select(r => new OriginalRoom(new Valve(r.Valve, r.FlowRate))));
        var startingRoom = allRooms.GetRoomByValveName("AA");
        scannedOutput.ScannedRooms.ForEach(sr =>
        {
            sr.ConnectedValves.ForEach(roomName =>
            {
                OriginalRoom.Connect(allRooms.GetRoomByValveName(sr.Valve), allRooms.GetRoomByValveName(roomName));
            });
        });
        return new CompleteNetwork(allRooms, startingRoom);
    }

    CompleteNetwork(AllRooms allRooms, OriginalRoom startingRoom)
    {
        Rooms = allRooms;
        _startingRoom = startingRoom;
    }

    public IEnumerable<CandidateSequence> GetAllCandidateSequences(int timeToComplete)
    {
        return GetAllCandidateSequences(timeToComplete,
            new CandidateSequence(Rooms, timeToComplete, _startingRoom, Array.Empty<IOperation>(), new HashSet<string>(), new HashSet<OriginalRoom>()));
    }

    IEnumerable<CandidateSequence> GetAllCandidateSequences(int timeRemaining, CandidateSequence sequenceSoFar)
    {
        if (timeRemaining == 0)
        {
            yield return sequenceSoFar;
            yield break;
        }
        var possibleOperations = sequenceSoFar.GetPossibleNextOperations(timeRemaining);
        foreach (var possibleOperation in possibleOperations)
        {
            var sequenceWithNextStep = sequenceSoFar.AddOperation(possibleOperation);
            foreach (var nextSequence
                     in GetAllCandidateSequences(timeRemaining - 1, sequenceWithNextStep))
            {
                yield return nextSequence;
            }
        }
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