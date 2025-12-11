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
        var input = Contents;
        var lineLength = input.IndexOf((byte)'\n');
        var lineCount = input.Count((byte)'\n');
        long total = 0;

        List<int> values = [];
        for (int i = lineLength - 1; i >= 0; i--)
        {
            int value = 0;
            for (int j = 0; j < lineCount - 1; j++)
            {
                var digit = input[j * (lineLength + 1) + i] - '0';
                if ((uint)digit <= 9u)
                {
                    value = 10 * value + digit;
                }
            }

            values.Add(value);
            var op = input[(lineCount - 1) * (lineLength + 1) + i];
            switch (op)
            {
                case (byte)'*':
                    long product = 1;
                    foreach (var num in values)
                    {
                        product *= num;
                    }
                    total += product;
                    values.Clear();
                    i--;
                    break;
                case (byte)'+':
                    foreach (var num in values)
                    {
                        total += num;
                    }
                    values.Clear();
                    i--;
                    break;
            }
        }

        return total.ToString();
    }
}
