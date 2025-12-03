namespace Aoc2025;

public partial class Day03 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method | Mean      | Error    | StdDev   |
     * |------- |----------:|---------:|---------:|
     * | Solve1 |  41.79 us | 0.056 us | 0.080 us |
     * | Solve2 | 211.80 us | 0.298 us | 0.446 us |
     */

    public override void ParseData()
    {
        // No common parsing logic
    }

    [Benchmark]
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

    private const int RequiredDigits = 12;

    [Benchmark]
    public override string Solve2()
    {
        var parser = new Parser(Contents);
        long total = 0;

        Span<sbyte> digits = stackalloc sbyte[RequiredDigits];

        while (parser.ParseLine() is { IsEmpty: false} line)
        {
            digits.Fill(-1);
            digits[0] = (sbyte)(line.ParseOne() - '0');

            while (line.TryParseDigit(out int digit))
            {
                for (int i = Math.Max(0, RequiredDigits - line.RemainingLength - 1); i < RequiredDigits; i++)
                {
                    if (digit > digits[i])
                    {
                        digits[i] = (sbyte)digit;
                        digits[(i + 1)..].Fill(-1);
                        break;
                    }
                }
            }

            long value = digits[0];
            for (int i = 1; i < RequiredDigits; i++)
            {
                value = 10*value + digits[i];
            }
            total += value;
        }

        return total.ToString();
    }
}
