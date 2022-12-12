using System.Numerics;

namespace AdventOfCode;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
    {
        List<T> currentBatch = new();
        foreach (var item in source)
        {
            currentBatch.Add(item);
            if (currentBatch.Count == batchSize)
            {
                yield return currentBatch;
                currentBatch = new List<T>();
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

    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        var index = 0;
        foreach (var item in source)
        {
            action(item, index);
            index++;
        }
    }
    
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }
    
    public static long Product(this IEnumerable<long> factors)
    {
        return factors.Aggregate(1L, (p, c) => p * c);
    }

    public static int Product(this IEnumerable<int> factors)
    {
        return factors.Aggregate(1, (p, c) => p * c);
    }
}