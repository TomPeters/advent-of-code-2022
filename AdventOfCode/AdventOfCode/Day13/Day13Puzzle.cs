namespace AdventOfCode.Day13;

public class Day13Puzzle
{
    public static int GetSumOfIndicesOfCorrectlyOrderedPairs(PacketPair[] packetPairs)
    {
        return packetPairs.Select((pair, i) => (pair, i)).Where(tuple =>
        {
            var (pair, _) = tuple;
            return pair.IsInCorrectOrder();
        }).Select((tuple) => tuple.i).Sum();
    }
}

public record PacketPair(IPacket Packet1, IPacket Packet2)
{
    public bool IsInCorrectOrder() => Packet1.CompareTo(Packet2) < 0;
}

public interface IPacket : IComparable<IPacket>
{
}

public class IntegerPacket : IPacket
{
    private readonly int _value;

    public IntegerPacket(int value)
    {
        _value = value;
    }

    public int CompareTo(IPacket? other)
    {
        if (other is IntegerPacket integerPacket)
        {
            return _value.CompareTo(integerPacket._value);
        }
        if (other is ListPacket listPacket)
        {
            return new ListPacket(new[] { this }).CompareTo(listPacket);
        }

        throw new NotImplementedException("Other packet types not supported");
    }
}

public class ListPacket : IPacket
{
    private readonly IPacket[] _packets;

    public ListPacket(IPacket[] packets)
    {
        _packets = packets;
    }

    public int CompareTo(IPacket? other)
    {
        if (other is IntegerPacket integerPacket)
        {
            return this.CompareTo(new ListPacket(new[] { integerPacket }));
        }

        if (other is ListPacket listPacket)
        {
            var comparisons = _packets.Zip(listPacket._packets, (packet, packet1) => packet.CompareTo(packet1));
            var firstNonEqualComparison = comparisons.FirstOrDefault(c => c != 0);
            if (firstNonEqualComparison != 0) return firstNonEqualComparison;
            // If they were all equal, compare the lengths to see which was shorter
            return _packets.Length.CompareTo(listPacket._packets.Length);
        }

        throw new NotImplementedException("Unsupported packet type");
    }
}
