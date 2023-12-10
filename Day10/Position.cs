using System.Diagnostics;

internal record struct Position(int X, int Y)
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Position LeftNeighbor => new(this.X - 1, this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Position RightNeighbor => new(this.X + 1, this.Y);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Position UpNeighbor => new(this.X, this.Y - 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public readonly Position DownNeighbor => new(this.X, this.Y + 1);

    public override string ToString()
    {
        return $"({this.X},{this.Y})";
    }
}
