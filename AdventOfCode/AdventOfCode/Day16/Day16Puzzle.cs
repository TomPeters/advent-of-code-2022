namespace AdventOfCode.Day16;

public class Day16Puzzle
{
    public static int GetTheMostPressureThatCanBeReleased(ScannedOutput scannedOutput)
    {
        var network = Network.CreateNetwork(scannedOutput);
        return 0;
    }
}

public class Network
{
    private readonly Room _startingRoom;

    public static Network CreateNetwork(ScannedOutput scannedOutput)
    {
        var allRooms = scannedOutput.ScannedRooms.Select(r => new Room(new Valve(r.Valve, r.FlowRate))).ToDictionary(r => r.Valve.Name);
        var startingRoom = allRooms["AA"];
        scannedOutput.ScannedRooms.ForEach(sr =>
        {
            sr.ConnectedValves.ForEach(roomName =>
            {
                Room.Connect(allRooms[sr.Valve], allRooms[roomName]);
            });
        });
        return new Network(startingRoom);
    }

    Network(Room startingRoom)
    {
        _startingRoom = startingRoom;
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

public record Valve(string Name, int FlowRate);

public record ScannedOutput(ScannedRoom[] ScannedRooms);

public record ScannedRoom(string Valve, int FlowRate, string[] ConnectedValves);