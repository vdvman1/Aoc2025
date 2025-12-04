namespace Aoc2025;

public partial class Day04 : DayBase
{
    private readonly List<bool[]> Grid = [];

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

    public override string Solve1()
    {
        int reachable = 0;

        for (int y = 0; y < Grid.Count; y++)
        {
            var row = Grid[y];
            for (int x = 0; x < row.Length; x++)
            {
                if (row[x] && IsReachable(x, y))
                {
                    reachable++;
                }
            }
        }

        return reachable.ToString();
    }

    private bool IsReachable(int x, int y)
    {
        int count = 0;
        foreach (var (_, hasPaper) in Grid.Adjacent8(new(x, y)))
        {
            if (hasPaper)
            {
                count++;
                if (count >= 4)
                {
                    return false;
                }
            }
        }
        return true;
    }

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
