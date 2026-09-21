namespace GeometryDrawing.Core;

/// <summary>
/// Общий контракт фигур, состоящих из точек и поддерживающих перемещение.
/// </summary>
public interface IFigure
{
    IReadOnlyList<Point2D> Points { get; }

    void AddX(int x);

    void AddY(int y);
}

