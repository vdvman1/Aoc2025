namespace Aoc2025;

public partial class Day07 : DayBase
{
    public override void ParseData()
    {
        // No common parsing logic
    }

    public override string Solve1()
    {
        var grid = new GridView<byte>(Contents, (byte)'\n');
        Span<bool> prevRow = stackalloc bool[grid.Width];
        prevRow.Clear();
        Span<bool> nextRow = stackalloc bool[grid.Width];
        nextRow.Clear();

        int total = 0;

        var start = grid.Row(0).IndexOf((byte)'S');
        prevRow[start] = true;

        for (int y = 1; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (!prevRow[x]) continue;

                if (grid[x, y] == '^')
                {
                    total++;
                    nextRow[x - 1] = true;
                    nextRow[x + 1] = true;
                }
                else
                {
                    nextRow[x] = true;
                }
            }

            // Swap row buffers
            var temp = prevRow;
            prevRow = nextRow;
            nextRow = temp;

            nextRow.Clear();
        }

        return total.ToString();
    }

    public override string Solve2()
    {
        var grid = new GridView<byte>(Contents, (byte)'\n');
        var cache = new long[grid.Width, grid.Height];
        var start = grid.Row(0).IndexOf((byte)'S');
        return CountPaths(grid, cache, start, 0).ToString();
    }

    private static long CountPaths(GridView<byte> grid, long[,] cache, int x, int y)
    {
        if (cache[x, y] > 0)
        {
            return cache[x, y];
        }

        var startY = y;
        do
        {
            y++;
            if (y >= grid.Height)
            {
                for (; startY < y; startY++)
                {
                    cache[x, startY] = 1;
                }
                return 1;
            }
        } while (grid[x, y] != (byte)'^');

        var count = CountPaths(grid, cache, x - 1, y) + CountPaths(grid, cache, x + 1, y);
        for (; startY <= y; startY++)
        {
            cache[x, startY] = count;
        }

        return count;
    }
}
