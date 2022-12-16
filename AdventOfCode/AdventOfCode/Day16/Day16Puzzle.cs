namespace AdventOfCode.Day16;

public class Day16Puzzle
{
    public static int GetTheMostPressureThatCanBeReleased(ScannedOutput scannedOutput)
    {
        var network = Network.CreateNetwork(scannedOutput);
        var allCandidateSequences = network.GetAllCandidateSequences(30).Where(s => s.CompleteSequenceIsValid());
        return allCandidateSequences.Max(s => s.GetTotalPressureReleased());
    }
}

public class CandidateSequence
{
    private readonly Room[] _allRooms;
    private readonly int _totalTimeToComplete;
    public Room LastRoom { get; }
    private readonly IOperation[] _operations;

    public CandidateSequence(Room[] allRooms, int totalTimeToComplete, Room lastRoom, IEnumerable<IOperation> operations)
    {
        _allRooms = allRooms;
        _totalTimeToComplete = totalTimeToComplete;
        LastRoom = lastRoom;
        _operations = operations.ToArray();
    }

    public CandidateSequence AddOperation(IOperation newOperation)
    {
        if (newOperation is MoveToRoomOperation moveToRoomOperation)
        {
            return new CandidateSequence(_allRooms, _totalTimeToComplete,  moveToRoomOperation.NewRoom, _operations.Concat(new [] { newOperation }));
        }
        return new CandidateSequence(_allRooms, _totalTimeToComplete, LastRoom, _operations.Concat(new [] { newOperation }));
    }

    public int GetTotalPressureReleased() => _operations.Sum(o => o.GetPressureReleased());

    public bool CanOpenValve(string valveName) =>
        _operations.Where(o => o is TurnOnValveInRoomOperation)
            .Cast<TurnOnValveInRoomOperation>()
            .All(o => o.Room.Valve.Name != valveName);
    
    public bool AnyValvesLeftToOpen()
    {
        return _allRooms.Where(r => r.Valve.HasAnyPressure()).Any(r => CanOpenValve(r.Valve.Name));
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
            var roomsVisitedSinceLastValveOpened = _operations.Reverse().TakeWhile(o => o is MoveToRoomOperation moveToRoomOperation).Cast<MoveToRoomOperation>().Select(o => o.NewRoom).ToHashSet();
            foreach (var connectedRoom in currentRoom.GetConnectedRooms().Where(r => !roomsVisitedSinceLastValveOpened.Contains(r)))
            {
                yield return new MoveToRoomOperation(connectedRoom);
            }
        }
    }
}

public interface IOperation
{
    int GetPressureReleased();
}

public class MoveToRoomOperation : IOperation
{
    public Room NewRoom { get; }

    public MoveToRoomOperation(Room newRoom)
    {
        NewRoom = newRoom;
    }

    public int GetPressureReleased() => 0;
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
}

public class DoNothingOperation : IOperation
{
    public int GetPressureReleased() => 0;
}

public class Network
{
    private readonly Room _startingRoom;
    private static Room[] _allRooms;

    public static Network CreateNetwork(ScannedOutput scannedOutput)
    {
        _allRooms = scannedOutput.ScannedRooms.Select(r => new Room(new Valve(r.Valve, r.FlowRate))).ToArray();
        var allRoomsLookup = _allRooms.ToDictionary(r => r.Valve.Name);
        var startingRoom = allRoomsLookup["AA"];
        scannedOutput.ScannedRooms.ForEach(sr =>
        {
            sr.ConnectedValves.ForEach(roomName =>
            {
                Room.Connect(allRoomsLookup[sr.Valve], allRoomsLookup[roomName]);
            });
        });
        return new Network(startingRoom);
    }

    Network(Room startingRoom)
    {
        _startingRoom = startingRoom;
    }

    public IEnumerable<CandidateSequence> GetAllCandidateSequences(int timeToComplete)
    {
        return GetAllCandidateSequences(timeToComplete,
            new CandidateSequence(_allRooms, timeToComplete, _startingRoom, Array.Empty<IOperation>()));
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