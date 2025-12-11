namespace Aoc2025;

public partial class Day06 : DayBase
{
    public override void ParseData()
    {
        // No common parsing
    }

    public override string Solve1()
    {
        var parser = new Parser(Contents);
        List<List<int>> problemValues = [];
        long total = 0;

        var line = parser.ParseLine();
        while (true)
        {
            line.SkipWhitespace();

            if (line.Peek() is (byte)'*' or (byte)'+') break;

            for (int i = 0; !line.IsEmpty; i++)
            {
                var num = line.ParsePosInt();
                if (i < problemValues.Count)
                {
                    problemValues[i].Add(num);
                }
                else
                {
                    problemValues.Add([num]);
                }
                line.SkipWhitespace();
            }

            line = parser.ParseLine();
        }

        foreach (var problem in problemValues)
        {
            if (line.ParseOne() == '*')
            {
                long product = 1;
                foreach (var value in problem)
                {
                    product *= value;
                }
                total += product;
            }
            else
            {
                foreach (var value in problem)
                {
                    total += value;
                }
            }

            line.SkipWhitespace();
        }

        return total.ToString();
    }

    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
