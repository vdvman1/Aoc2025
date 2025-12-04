namespace Aoc2025;

public partial class Day04 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method    | Mean        | Error     | StdDev    | Median      |
     * |---------- |------------:|----------:|----------:|------------:|
     * | ParseData |    16.33 us |  0.287 us |  0.429 us |    16.09 us |
     * | Solve1    |   184.19 us |  0.251 us |  0.368 us |   184.24 us |
     * | Solve2    | 2,845.79 us | 11.582 us | 15.854 us | 2,847.83 us |
     */

    private readonly List<bool[]> Grid = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents);
        Grid.Clear();

        while (!parser.IsEmpty && parser.ParseLine() is { Remainder: var line })
        {
            var row = new bool[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                row[i] = line[i] == '@';
            }
            Grid.Add(row);
        }
    }

    [Benchmark]
    public override string Solve1()
    {
        int reachable = 0;

        bool[] above;
        var row = Grid[0];
        var below = Grid[1];

        var rowEnd = row.Length - 1;
        var gridEnd = Grid.Count - 1;

        if (row[0])
        {
            // 0,0 has only 3 neighbours, so guaranteed to be reachable
            reachable++;
        }

        for (int x = 1; x < rowEnd; x++)
        {
            if (row[x])
            {
                var count = 0;
                if (row[x - 1])
                {
                    count++;
                }

                if (row[x + 1])
                {
                    count++;
                }

                if (below[x - 1])
                {
                    count++;
                }

                if (below[x])
                {
                    if (count >= 3) continue;
                    count++;
                }

                if (below[x + 1] && count >= 3) continue;

                reachable++;
            }
        }

        if (row[rowEnd])
        {
            // ^1, 0 has only 3 neighbours, so guaranteed to be reachable
            reachable++;
        }

        for (int y = 1; y < gridEnd; y++)
        {
            above = row;
            row = below;
            below = Grid[y + 1];

            if (row[0])
            {
                var count = 0;
                if (above[0])
                {
                    count++;
                }

                if (above[1])
                {
                    count++;
                }

                if (row[1])
                {
                    count++;
                }

                if (below[0])
                {
                    if (count >= 3) goto middleXs;
                    count++;
                }

                if (below[1] && count >= 3) goto middleXs;

                reachable++;
            }

        middleXs:
            for (int x = 1; x < rowEnd; x++)
            {
                if (row[x])
                {
                    var count = 0;
                    if (above[x - 1])
                    {
                        count++;
                    }

                    if (above[x])
                    {
                        count++;
                    }

                    if (above[x + 1])
                    {
                        count++;
                    }

                    if (row[x - 1])
                    {
                        if (count >= 3) continue;
                        count++;
                    }

                    if (row[x + 1])
                    {
                        if (count >= 3) continue;
                        count++;
                    }

                    if (below[x - 1])
                    {
                        if (count >= 3) continue;
                        count++;
                    }

                    if (below[x])
                    {
                        if (count >= 3) continue;
                        count++;
                    }

                    if (below[x + 1] && count >= 3) continue;

                    reachable++;
                }
            }

            if (row[rowEnd])
            {
                var count = 0;
                if (above[rowEnd - 1])
                {
                    count++;
                }

                if (above[rowEnd])
                {
                    count++;
                }

                if (row[rowEnd - 1])
                {
                    count++;
                }

                if (below[rowEnd - 1])
                {
                    if (count >= 3) continue;
                    count++;
                }

                if (below[rowEnd] && count >= 3) continue;

                reachable++;
            }
        }

        above = row;
        row = below;

        if (row[0])
        {
            // 0, ^1 has only 3 neighbours, so guaranteed to be reachable
            reachable++;
        }

        for (int x = 1; x < rowEnd; x++)
        {
            if (row[x])
            {
                var count = 0;
                if (above[x - 1])
                {
                    count++;
                }

                if (above[x])
                {
                    count++;
                }

                if (above[x + 1])
                {
                    count++;
                }

                if (row[x - 1])
                {
                    if (count >= 3) continue;
                    count++;
                }

                if (row[x + 1] && count >= 3) continue;

                reachable++;
            }
        }

        if (row[rowEnd])
        {
            // ^1, ^1 has only 3 neighbours, so guaranteed to be reachable
            reachable++;
        }

        return reachable.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        // Copy grid to avoid mutating original
        var grid = Grid.ConvertAll<bool[]>(row => [..row]);

        int reachable = 0;
        Stack<VectorI2d> locationsToCheck = new();

        for (int y = 0; y < grid.Count; y++)
        {
            var row = grid[y];
            for (int x = 0; x < row.Length; x++)
            {
                if (TryRemove(new(x, y), grid, locationsToCheck, includeAll: false))
                {
                    reachable++;
                }
            }
        }

        while (locationsToCheck.TryPop(out var startPos))
        {
            if (TryRemove(startPos, grid, locationsToCheck, includeAll: true))
            {
                reachable++;
            }
        }

        return reachable.ToString();
    }

    private static bool TryRemove(VectorI2d startPos, List<bool[]> grid, Stack<VectorI2d> locationsToCheck, bool includeAll)
    {
        ref bool startCell = ref grid.RefAt(startPos);
        if (!startCell) return false;

        Span<VectorI2d> tempPotentialLocations = stackalloc VectorI2d[8];
        int tempPotentialLocationCount = 0;

        int count = 0;
        foreach (var (pos, hasPaper) in grid.Adjacent8(startPos))
        {
            if (!hasPaper) continue;

            ++count;
            if (count >= 4) return false;

            if (includeAll || pos.IsScannedEarlierThan(startPos))
            {
                tempPotentialLocations[tempPotentialLocationCount] = pos;
                tempPotentialLocationCount++;
            }
        }

        // This paper was reachable, so remove it and mark surrounding cells for future checking for reachability
        startCell = false;
        for (int i = 0; i < tempPotentialLocationCount; i++)
        {
            locationsToCheck.Push(tempPotentialLocations[i]);
        }

        return true;
    }
}
