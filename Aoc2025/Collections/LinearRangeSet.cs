using System.Numerics;

namespace Aoc2025.Collections;

public class LinearRangeSet<T> where T : IBinaryInteger<T>
{
    private readonly List<InclusiveIntegerRange<T>> Ranges = [];

    public void Clear() => Ranges.Clear();

    public void AddRange(InclusiveIntegerRange<T> range)
    {
        for (int i = 0; i < Ranges.Count; i++)
        {
            var currRange = Ranges[i];

            // Completely before current range, insert before
            if (range.End < currRange.Start - T.One)
            {
                Ranges.Insert(i, range);
                return;
            }

            // Connects with current range, merge
            if (range.Start <= currRange.End + T.One)
            {
                // Connects with next range(s), merge
                while (i + 1 < Ranges.Count && Ranges[i + 1] is var nextRange && range.End >= nextRange.Start - T.One)
                {
                    Ranges.RemoveAt(i + 1); // TODO: optimise multiple removals

                    // Doesn't extend beyond the next range, complete the merge
                    if (range.End <= nextRange.End)
                    {
                        Ranges[i] = new(currRange.Start, nextRange.End);
                        return;
                    }

                    // Extends beyond the next range, check next-next page
                }

                // Extends beyond the entire list, or doesn't extend into the next range, merge with current range
                Ranges[i] = new(T.Min(range.Start, currRange.Start), T.Max(range.End, currRange.End));
                return;
            }

            // Completely after current range, check next range
        }

        // Reached the end of the list without being less, so insert at end
        Ranges.Add(range);
    }

    public bool Contains(T value)
    {
        foreach (var range in Ranges)
        {
            if (value < range.Start)
            {
                return false; // Short circuit, as ranges are sorted
            }

            if (value <= range.End)
            {
                return true;
            }
        }

        return false;
    }

    public T Count()
    {
        T count = T.AdditiveIdentity;
        foreach (var range in Ranges)
        {
            count += range.Count;
        }

        return count;
    }
}
