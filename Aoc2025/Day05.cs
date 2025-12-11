using Aoc2025.Collections;

namespace Aoc2025;

public partial class Day05 : DayBase
{
    private readonly LinearRangeSet<long> FreshRanges = new();
    private readonly List<long> Ingredients = [];

    public override void ParseData()
    {
        var parser = new Parser(Contents);
        FreshRanges.Clear();

        while (parser.ParseLine() is { IsEmpty: false } line)
        {
            var start = line.ParsePosLong();
            line.MoveNext(); // skip '-'
            var end = line.ParsePosLong();

            FreshRanges.AddRange(new(start, end));
        }

        Ingredients.Clear();
        while (!parser.IsEmpty)
        {
            Ingredients.Add(parser.ParsePosLong());
            parser.MoveNext(); // skip newline
        }
    }

    public override string Solve1() => Ingredients.Count(FreshRanges.Contains).ToString();

    public override string Solve2() => FreshRanges.Count().ToString();
}
