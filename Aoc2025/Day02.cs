namespace Aoc2025;

public partial class Day02 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method | Mean      | Error     | StdDev    |
     * |------- |----------:|----------:|----------:|
     * | Solve1 |  6.839 ms | 0.0059 ms | 0.0083 ms |
     * | Solve2 | 46.302 ms | 0.0786 ms | 0.1102 ms |
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
                for (; value < digitRangeEnd && value <= rangeEnd; value++)
                {
                    var (top, bottom) = Math.DivRem(value, digitSplit);
                    if (top == bottom)
                    {
                        total += value;
                    }
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

    [Benchmark]
    public override string Solve2()
    {
        var parser = new Parser(Contents);

        long total = 0;
        do
        {
            var digitCount = parser.Pos;
            var value = parser.ParsePosLong(); // range start
            digitCount = parser.Pos - digitCount;

            parser.MoveNext(); // skip '-'

            var rangeEnd = parser.ParsePosLong();

            parser.MoveNext(); // skip ','

            long digitRangeEnd;
            if (digitCount == 1)
            {
                if (rangeEnd < 10)
                {
                    continue;
                }

                value = 10;
                digitRangeEnd = 10;
            }
            else
            {
                digitRangeEnd = 10L.Pow(digitCount); // e.g. starts at 2 digits, 10^2 = 100, the first 3 digit number
            }
                
            do
            {
                var factors = GetDigitCountFactors(digitCount).ToList();
                for (; value < digitRangeEnd && value <= rangeEnd; value++)
                {
                    if (HasPattern(value, factors))
                    {
                        total += value;
                    }
                }

                digitRangeEnd *= 10;
                digitCount++;
            } while (value <= rangeEnd);
        } while (!parser.IsEmpty);
        return total.ToString();
    }

    private static IEnumerable<(int, long)> GetDigitCountFactors(int digitCount)
    {
        bool prime = true;
        foreach (var (left, right) in digitCount.Factors().Skip(1))
        {
            prime = false;
            yield return (left, 10L.Pow(right));
            yield return (right, 10L.Pow(left));
        }

        if (prime)
        {
            yield return (digitCount, 10L);
        }
    }

    private static bool HasPattern(long value, List<(int, long)> digitCountFactors)
    {
        foreach (var (factorCount, factor) in digitCountFactors)
        {
            if (HasPatternWithFactor(value, factorCount, factor))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasPatternWithFactor(long value, int factorCount, long factor)
    {
        (value, var pattern) = Math.DivRem(value, factor);
        for (int i = 1; i < factorCount; i++)
        {
            (value, var nextPattern) = Math.DivRem(value, factor);
            if (nextPattern != pattern) return false;
        }

        return true;
    }
}
