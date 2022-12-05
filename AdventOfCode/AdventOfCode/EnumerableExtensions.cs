namespace AdventOfCode;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
    {
        List<T> currentBatch = new();
        using (var enumerator = source.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                currentBatch.Add(enumerator.Current);
                if (currentBatch.Count == batchSize)
                {
                    yield return currentBatch;
                    currentBatch = new List<T>();
                }
            }
        }

        if (currentBatch.Any())
        {
            yield return currentBatch;
        }
    }

    /// <summary>
    /// Includes the start and finish numbers.
    /// Finish must be greater than start.
    /// </summary>
    public static IEnumerable<int> BoundedRange(int start, int finish)
    {
        return Enumerable.Range(start, finish - start + 1);
    }
}