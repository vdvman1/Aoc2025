using System.Numerics;

namespace Aoc2025.Collections;

public record struct InclusiveIntegerRange<T>(T Start, T End) where T : IBinaryInteger<T>
{
    public readonly bool Contains(T value) => Start <= value && value <= End;

    public readonly T Count => End - Start + T.One;
}
