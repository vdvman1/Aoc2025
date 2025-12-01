namespace Aoc2025;

public partial class Day01 : DayBase
{
    /* 
     * Measured performance:
     * 
     * 
     */

    [Benchmark]
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

            dial = (dial + movement).FloorMod(100);
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
        throw new NotImplementedException();
    }
}
