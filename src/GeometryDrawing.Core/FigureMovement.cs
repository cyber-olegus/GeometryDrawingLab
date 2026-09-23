namespace GeometryDrawing.Core;

public static class FigureMovement
{
    /// <summary>
    /// Перемещает фигуру, если после сдвига все её точки останутся в заданной области.
    /// </summary>
    public static bool TryMoveWithin(
        IFigure figure,
        int deltaX,
        int deltaY,
        int areaWidth,
        int areaHeight)
    {
        ArgumentNullException.ThrowIfNull(figure);

        if (areaWidth < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(areaWidth));
        }

        if (areaHeight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(areaHeight));
        }

        bool remainsInside = figure.Points.All(point =>
        {
            long newX = (long)point.X + deltaX;
            long newY = (long)point.Y + deltaY;
            return newX >= 0 && newX <= areaWidth
                && newY >= 0 && newY <= areaHeight;
        });

        if (!remainsInside)
        {
            return false;
        }

        figure.AddX(deltaX);
        figure.AddY(deltaY);
        return true;
    }
}

