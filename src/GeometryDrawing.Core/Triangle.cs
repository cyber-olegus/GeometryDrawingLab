namespace GeometryDrawing.Core;

public sealed class Triangle : IFigure
{
    private readonly Point2D[] _points;

    public Point2D P1 => _points[0];

    public Point2D P2 => _points[1];

    public Point2D P3 => _points[2];

    public IReadOnlyList<Point2D> Points => _points;

    public Triangle(Point2D p1, Point2D p2, Point2D p3)
    {
        ArgumentNullException.ThrowIfNull(p1);
        ArgumentNullException.ThrowIfNull(p2);
        ArgumentNullException.ThrowIfNull(p3);

        if (SignedDoubleArea(p1, p2, p3) == 0)
        {
            throw new ArgumentException("Точки треугольника не должны лежать на одной прямой.");
        }

        _points = [p1, p2, p3];
    }

    public void AddX(int x)
    {
        foreach (Point2D point in _points)
        {
            point.AddX(x);
        }
    }

    public void AddY(int y)
    {
        foreach (Point2D point in _points)
        {
            point.AddY(y);
        }
    }

    private static long SignedDoubleArea(Point2D p1, Point2D p2, Point2D p3)
    {
        return (long)(p2.X - p1.X) * (p3.Y - p1.Y)
             - (long)(p2.Y - p1.Y) * (p3.X - p1.X);
    }
}

