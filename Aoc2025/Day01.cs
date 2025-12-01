namespace Aoc2025;

public partial class Day01 : DayBase
{
    private const int DialSize = 100;

    /* 
     * Measured performance:
     * 
     * | Method | Mean     | Error    | StdDev   |
     * |------- |---------:|---------:|---------:|
     * | Solve1 | 43.30 us | 0.119 us | 0.174 us |
     * | Solve2 | 47.75 us | 0.107 us | 0.160 us |
     */

    public override void ParseData()
    {
        // No common parsing logic
    }

    [Benchmark]
    public override string Solve1()
    {
        var parser = new Parser(Contents);

        var dial = 50;
        var zeroCount = 0;
        while (!parser.IsEmpty)
        {
            var movement = parser.ParseOne() == 'L' ? -1 : 1;
            movement *= parser.ParsePosInt();
            
            parser.MoveNext(); // Skip newline

            dial = (dial + movement).FloorMod(DialSize);
            if (dial == 0)
            {
                zeroCount++;
            }
        }

        return zeroCount.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        var parser = new Parser(Contents);

        var dial = 50;
        var zeroCount = 0;
        while (!parser.IsEmpty)
        {
            var movement = parser.ParseOne() == 'L' ? -1 : 1;
            var absMovement = parser.ParsePosInt();
            (var repeats, absMovement) = int.DivRem(absMovement, DialSize);
            zeroCount += repeats;
            movement *= absMovement;

            parser.MoveNext(); // Skip newline

            if (dial == 0)
            {
                // Can't cross zero if already at zero, unless the absolute movement was >= DialSize, which was handled with the earlier DivRem logic
                dial = (dial + movement).FloorMod(DialSize);
            }
            else
            {
                dial += movement;
                switch (dial)
                {
                    case < 0:
                        zeroCount++;
                        dial += DialSize;
                        break;
                    case 0:
                        zeroCount++;
                        break;
                    case >= DialSize:
                        zeroCount++;
                        dial -= DialSize;
                        break;
                }
            }
        }

        return zeroCount.ToString();
    }
}
