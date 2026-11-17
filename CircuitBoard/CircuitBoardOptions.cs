using CircuitSim.Numerics;

namespace CircuitSim;

public class CircuitBoardOptions
{
    // Scale represents the thickness of a wire, which can be zoomed by the user
    public static readonly int MinScale = 4;
    public static readonly int MaxScale = 40;
    public int Scale = 5;

    // Where the origin of the board has been dragged to
    public Vector2Int Offset = Vector2Int.Zero;
}
