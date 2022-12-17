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
    private readonly HashSet<Room> _roomsVisitedSinceLastValveOpened;
    public Room LastRoom { get; }
    private readonly IOperation[] _operations;

    public CandidateSequence(AllRooms allRooms, int totalTimeToComplete, Room lastRoom, IEnumerable<IOperation> operations, HashSet<string> openedValves, HashSet<Room> roomsVisitedSinceLastValveOpened)
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
        HashSet<Room> roomsSinceLastOpenValveOperation = openedValve != null ? new HashSet<Room>() :
            newRoom == null ? _roomsVisitedSinceLastValveOpened : new HashSet<Room>(_roomsVisitedSinceLastValveOpened.Concat(new [] { newRoom }));
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
    Room? GetNewRoom();
    string? GetValveOpened();
}

public class MoveToRoomOperation : IOperation
{
    public Room NewRoom { get; }

    public MoveToRoomOperation(Room newRoom)
    {
        NewRoom = newRoom;
    }

    public int GetPressureReleased() => 0;
    public Room? GetNewRoom()
    {
        return NewRoom;
    }

    public string? GetValveOpened() => null;
}

public class TurnOnValveInRoomOperation : IOperation
{
    public Room Room { get; }
    private readonly int _timeAvailableForValveToReleasePressure;

    public TurnOnValveInRoomOperation(Room room, int timeAvailableForValveToReleasePressure)
    {
        Room = room;
        _timeAvailableForValveToReleasePressure = timeAvailableForValveToReleasePressure;
    }

    public int GetPressureReleased() => _timeAvailableForValveToReleasePressure * Room.Valve.FlowRate;
    public Room? GetNewRoom() => null;

    public string? GetValveOpened() => Room.Valve.Name;
}

public class DoNothingOperation : IOperation
{
    public int GetPressureReleased() => 0;
    public Room? GetNewRoom() => null;

    public string? GetValveOpened() => null;
}

public class AllRooms
{
    private readonly string[] _valvesWithAnyPressure;
    private readonly Dictionary<string, Room> _roomsByValveName;

    public AllRooms(IEnumerable<Room> rooms)
    {
        var roomsArray = rooms.ToArray();
        _valvesWithAnyPressure = roomsArray.Where(r => r.Valve.HasAnyPressure()).Select(r => r.Valve.Name).ToArray();
        _roomsByValveName = roomsArray.ToDictionary(r => r.Valve.Name);
    }

    public string[] GetValvesThatHaveAnyPressure() => _valvesWithAnyPressure;

    public Room GetRoomByValveName(string valveName) => _roomsByValveName[valveName];
}

public class CompleteNetwork
{
    private readonly Room _startingRoom;
    private AllRooms _allRooms;

    public static CompleteNetwork CreateNetwork(ScannedOutput scannedOutput)
    {
        var allRooms = new AllRooms(scannedOutput.ScannedRooms.Select(r => new Room(new Valve(r.Valve, r.FlowRate))));
        var startingRoom = allRooms.GetRoomByValveName("AA");
        scannedOutput.ScannedRooms.ForEach(sr =>
        {
            sr.ConnectedValves.ForEach(roomName =>
            {
                Room.Connect(allRooms.GetRoomByValveName(sr.Valve), allRooms.GetRoomByValveName(roomName));
            });
        });
        return new CompleteNetwork(allRooms, startingRoom);
    }

    CompleteNetwork(AllRooms allRooms, Room startingRoom)
    {
        _allRooms = allRooms;
        _startingRoom = startingRoom;
    }

    public IEnumerable<CandidateSequence> GetAllCandidateSequences(int timeToComplete)
    {
        return GetAllCandidateSequences(timeToComplete,
            new CandidateSequence(_allRooms, timeToComplete, _startingRoom, Array.Empty<IOperation>(), new HashSet<string>(), new HashSet<Room>()));
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

public class Room
{
    protected bool Equals(Room other)
    {
        return Valve.Equals(other.Valve);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Room)obj);
    }

    public override int GetHashCode()
    {
        return Valve.GetHashCode();
    }

    public Valve Valve { get; init; }
    private readonly ISet<Room> _connectedRooms = new HashSet<Room>();

    public IEnumerable<Room> GetConnectedRooms() => _connectedRooms;

    public Room(Valve valve)
    {
        Valve = valve;
    }

    public static void Connect(Room firstRoom, Room secondRoom)
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