namespace Aoc2025.Utilities;

public readonly ref struct GridView<T> where T : IEquatable<T>
{
    private readonly ReadOnlySpan<T> Data;

    public readonly int Width;
    public readonly int Height;

    public GridView(ReadOnlySpan<T> data, T delimeter)
    {
        Data = data;
        Width = data.IndexOf(delimeter);
        Height = data.Count(delimeter);
    }

    public readonly T this[Index x, Index y] => Data[y.GetOffset(Height) * (Width + 1) + x.GetOffset(Width)];
}
