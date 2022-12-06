using System.Collections;

namespace AdventOfCode.Day6;

public static class Day6Puzzle
{
    public static int GetNumberOfCharactersProcessedBeforeFirstStartOfPacketMarker(DataStreamBuffer dataStreamBuffer)
    {
        return dataStreamBuffer.GetNumberOfCharactersProcessedBeforeFirstUniqueSequenceOfLength(4);
    }
    
    public static int GetNumberOfCharactersProcessedBeforeFirstStartOfMessageMarker(DataStreamBuffer dataStreamBuffer)
    {
        return dataStreamBuffer.GetNumberOfCharactersProcessedBeforeFirstUniqueSequenceOfLength(14);
    }
}

public record DataStreamBuffer(IEnumerable<char> Characters)
{
    public int GetNumberOfCharactersProcessedBeforeFirstUniqueSequenceOfLength(int sequenceLength)
    {
        var sequenceIndex = GetSequences(sequenceLength).Select((sequence, index) => (sequence, index))
            .First(tuple => tuple.sequence.AllCharactersAreUnique()).index;

        return sequenceIndex + sequenceLength;
    }

    IEnumerable<Sequence> GetSequences(int length)
    {
        var buffer = new Queue<char>();
        using var enumerator = Characters.GetEnumerator();
        
        while (enumerator.MoveNext())
        {
            buffer.Enqueue(enumerator.Current);
            if (buffer.Count == length)
            {
                yield return new Sequence(buffer.ToArray());
                buffer.Dequeue();
            }
        }
    } 
}

public record Sequence(char[] Characters)
{
    public bool AllCharactersAreUnique()
    {
        return Characters.Distinct().Count() == Characters.Length;
    }
}