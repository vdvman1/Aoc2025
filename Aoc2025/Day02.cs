namespace Aoc2025;

public partial class Day02 : DayBase
{
    public override void ParseData()
    {
        // No common parsing logic
    }

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

    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
