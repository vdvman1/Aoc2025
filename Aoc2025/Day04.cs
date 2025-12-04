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
            for (int x = 0; x < Grid[y].Length; x++)
            {
                var pos = new VectorI2d(x, y);
                if (Grid.At(pos) && IsReachable(pos))
                {
                    reachable++;
                }
            }
        }

        return reachable.ToString();
    }

    private bool IsReachable(VectorI2d pos)
    {
        int count = 0;
        foreach (var (_, hasPaper) in Grid.Adjacent8(pos))
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
        throw new NotImplementedException();
    }
}
