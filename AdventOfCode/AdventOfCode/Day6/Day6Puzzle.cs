using System.Collections;

namespace AdventOfCode.Day6;

public static class Day6Puzzle
{
    public static int GetNumberOfCharactersProcessedBeforeFirstStartOfPacketMarker(DataStreamBuffer dataStreamBuffer)
    {
        return dataStreamBuffer.GetNumberOfCharactersProcessBeforeFirstStartOfPacketMarker();
    }
}

public record DataStreamBuffer(IEnumerable<char> Characters)
{
    public int GetNumberOfCharactersProcessBeforeFirstStartOfPacketMarker()
    {
        var sequenceIndex = GetSequences().Select((sequence, index) => (sequence, index))
            .First((tuple) => tuple.sequence.IsStartOfPacketMarker()).index;

        return sequenceIndex + 4;
    }

    IEnumerable<Sequence> GetSequences()
    {
        var buffer = new Queue<char>();
        using var enumerator = Characters.GetEnumerator();
        
        while (enumerator.MoveNext())
        {
            buffer.Enqueue(enumerator.Current);
            if (buffer.Count == 4)
            {
                yield return new Sequence(buffer.ToArray());
                buffer.Dequeue();
            }
        }
    } 
}

public record Sequence(char[] Characters)
{
    public bool IsStartOfPacketMarker()
    {
        return Characters.Distinct().Count() == 4;
    }
}