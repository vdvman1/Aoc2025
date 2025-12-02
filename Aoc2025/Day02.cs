using System.Text.RegularExpressions;

namespace Aoc2025;

public partial class Day02 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method | Mean       | Error     | StdDev    |
     * |------- |-----------:|----------:|----------:|
     * | Solve1 |   6.871 ms | 0.0407 ms | 0.0597 ms |
     * | Solve2 | 493.317 ms | 0.8447 ms | 1.2114 ms |
     */

    public override void ParseData()
    {
        // No common parsing logic
    }

    [Benchmark]
    public override string Solve1()
    {
        var parser = new Parser(Contents);

        long total = 0;
        do
        {
            var digitCount = parser.Pos;
            var value = parser.ParsePosLong(); // range start
            digitCount = parser.Pos - digitCount;
            var digitRangeEnd = 10L.Pow(digitCount); // e.g. starts at 2 digits, 10^2 = 100, the first 3 digit number

            parser.MoveNext(); // skip '-'

            var rangeEnd = parser.ParsePosLong();

            parser.MoveNext(); // Skip ','

            var (halfDigits, remainder) = Math.DivRem(digitCount, 2);
            if (remainder == 1)
            {
                // Adjust to next even digit count
                value = digitRangeEnd;
                halfDigits++;
                digitRangeEnd *= 10;
            }

            var digitSplit = 10L.Pow(halfDigits);

            while (value <= rangeEnd)
            {
                while (value < digitRangeEnd && value <= rangeEnd)
                {
                    var (top, bottom) = Math.DivRem(value, digitSplit);
                    if (top == bottom)
                    {
                        total += value;
                    }
                    value++;
                }

                // value == digitRangeEnd, which is an odd number of digits
                // Skip to start of next even digit count
                value *= 10;
                digitRangeEnd *= 100;
                digitSplit *= 10;
            }
        } while (!parser.IsEmpty);
        return total.ToString();
    }

    [GeneratedRegex("""^(.*)\1+$""")]
    private static partial Regex RepeatedPatternRegex();

    [Benchmark]
    public override string Solve2()
    {
        var parser = new Parser(Contents);
        var repeatedPatternRegex = RepeatedPatternRegex();

        long total = 0;
        do
        {
            var value = parser.ParsePosLong(); // range start
            parser.MoveNext(); // skip '-'
            var rangeEnd = parser.ParsePosLong();
            parser.MoveNext(); // skip ','

            for (; value <= rangeEnd; value++)
            {
                if (repeatedPatternRegex.IsMatch(value.ToString()))
                {
                    total += value;
                }
            }
        } while (!parser.IsEmpty);

        return total.ToString();
    }
}
