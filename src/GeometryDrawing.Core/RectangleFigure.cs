namespace GeometryDrawing.Core;

/// <summary>
/// Прямоугольник, задаваемый левой верхней точкой, шириной и высотой.
/// </summary>
public sealed class RectangleFigure : IFigure
{
    private readonly Point2D[] _points;

    public Point2D P1 => _points[0];

    public Point2D P2 => _points[1];

    public Point2D P3 => _points[2];

    public Point2D P4 => _points[3];

    public int Width { get; }

    public int Height { get; }

    public bool IsSquare => Width == Height;

    public IReadOnlyList<Point2D> Points => _points;

    public RectangleFigure(Point2D startPoint, int width, int height)
    {
        ArgumentNullException.ThrowIfNull(startPoint);

        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Ширина должна быть больше нуля.");
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "Высота должна быть больше нуля.");
        }

        Width = width;
        Height = height;
        _points =
        [
            startPoint,
            new Point2D(startPoint.X + width, startPoint.Y),
            new Point2D(startPoint.X + width, startPoint.Y + height),
            new Point2D(startPoint.X, startPoint.Y + height)
        ];
    }

    public static RectangleFigure CreateSquare(Point2D startPoint, int side)
    {
        return new RectangleFigure(startPoint, side, side);
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
}

