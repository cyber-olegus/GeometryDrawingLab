namespace GeometryDrawing.Core;

/// <summary>
/// Точка на двумерной плоскости с целочисленными координатами.
/// </summary>
public sealed class Point2D
{
    public int X { get; private set; }

    public int Y { get; private set; }

    public Point2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void AddX(int x)
    {
        X += x;
    }

    public void AddY(int y)
    {
        Y += y;
    }

    public override string ToString()
    {
        return $"({X}; {Y})";
    }
}

