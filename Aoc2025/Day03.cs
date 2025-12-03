namespace Aoc2025;

public partial class Day03 : DayBase
{
    public override void ParseData()
    {
        // No common parsing logic
    }

    public override string Solve1()
    {
        var parser = new Parser(Contents);
        int total = 0;

        while (parser.ParseLine() is { IsEmpty: false } line)
        {
            int max0 = line.ParseOne() - '0';
            int max1 = -1;

            while (line.TryParseDigit(out int digit))
            {
                if (digit > max0 && !line.IsEmpty)
                {
                    max0 = digit;
                    max1 = -1;
                }
                else
                {
                    max1 = Math.Max(digit, max1);
                }
            }

            total += 10 * max0 + max1;
        }

        return total.ToString();
    }

    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
