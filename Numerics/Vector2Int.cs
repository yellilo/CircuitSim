using CircuitSim.Circuitry;
using System;
using System.Numerics;

namespace CircuitSim.Numerics;

public struct Vector2Int : IEquatable<Vector2Int>
{
    public int X;
    public int Y;

    public static readonly Vector2Int Zero = new(0);
    public static readonly Vector2Int One = new(1);

    public static readonly Vector2Int Right = new(1, 0);
    public static readonly Vector2Int Down = new(0, 1);
    public static readonly Vector2Int Left = -Right;
    public static readonly Vector2Int Up = -Down;

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Vector2Int(int uniform)
    {
        X = uniform;
        Y = uniform;
    }

    public Vector2Int(ComponentOrientation orientation)
    {
        Vector2Int vec;

        switch (orientation)
        {
            case ComponentOrientation.Down:
                vec = Down;
                break;
            case ComponentOrientation.Left:
                vec = Left;
                break;
            case ComponentOrientation.Up:
                vec = Up;
                break;
            default:
                vec = Right;
                break;
        }

        X = vec.X;
        Y = vec.Y;
    }

    // Rounding
    public static Vector2Int Round(Vector2 vec) => new(
        (int)MathF.Round(vec.X),
        (int)MathF.Round(vec.Y)
    );

    // Addition
    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);

    // Subtraction
    public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2Int operator -(Vector2Int vec) => new(-vec.X, -vec.Y);

    // Multiplication
    public static Vector2Int operator *(Vector2Int a, Vector2Int b) => new(a.X * b.X, a.Y * b.Y);
    public static Vector2Int operator *(Vector2Int vec, int scale) => new(vec.X * scale, vec.Y * scale);
    public static Vector2Int operator *(int scale, Vector2Int vec) => vec * scale;

    // Divison
    public static Vector2Int operator /(Vector2Int a, Vector2Int b) => new(a.X / b.X, a.Y / b.Y);
    public static Vector2Int operator /(Vector2Int vec, int divisor) => new(vec.X / divisor, vec.Y / divisor);

    // Equality
    public static bool operator ==(Vector2Int a, Vector2Int b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Vector2Int a, Vector2Int b) => !(a == b);
    public readonly bool Equals(Vector2Int other) => this == other;
    public override readonly bool Equals(object? obj) => obj is Vector2Int other && Equals(other);

    // Conversion
    public static explicit operator Vector2Int(Vector2 vec) => new((int)vec.X, (int)vec.Y);
    public static implicit operator Vector2(Vector2Int vec) => new(vec.X, vec.Y);

    public override readonly int GetHashCode() => HashCode.Combine(X, Y);
    public override readonly string ToString() => $"({X}, {Y})";
}