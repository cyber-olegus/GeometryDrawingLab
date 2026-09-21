using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using GeometryDrawing.Core;

namespace GeometryDrawing.Wpf;

public partial class MainWindow : Window
{
    private const int SceneWidth = 680;
    private const int SceneHeight = 520;
    private const int ScenePadding = 24;

    private readonly Random _random = new();
    private IFigure? _currentFigure;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void RandomTriangle_Click(object sender, RoutedEventArgs e)
    {
        Triangle? triangle = null;

        // Повторяем генерацию, если случайно получили точки на одной прямой.
        while (triangle is null)
        {
            try
            {
                triangle = new Triangle(RandomPoint(), RandomPoint(), RandomPoint());
            }
            catch (ArgumentException)
            {
                // Для случайных целых координат это редкий и безопасный повтор.
            }
        }

        _currentFigure = triangle;
        DrawCurrentFigure();
        StatusText.Text = "Создан случайный треугольник.";
    }

    private void RandomRectangle_Click(object sender, RoutedEventArgs e)
    {
        int width = _random.Next(70, 230);
        int height = _random.Next(70, 230);
        int x = _random.Next(ScenePadding, SceneWidth - ScenePadding - width);
        int y = _random.Next(ScenePadding, SceneHeight - ScenePadding - height);

        _currentFigure = new RectangleFigure(new Point2D(x, y), width, height);
        DrawCurrentFigure();
        StatusText.Text = $"Создан прямоугольник {width} × {height}.";
    }

    private void ClearScene_Click(object sender, RoutedEventArgs e)
    {
        Scene.Children.Clear();
        _currentFigure = null;
        StatusText.Text = "Холст очищен.";
    }

    private Point2D RandomPoint()
    {
        return new Point2D(
            _random.Next(ScenePadding, SceneWidth - ScenePadding),
            _random.Next(ScenePadding, SceneHeight - ScenePadding));
    }

    private void DrawCurrentFigure()
    {
        Scene.Children.Clear();

        if (_currentFigure is Triangle triangle)
        {
            DrawTriangle(triangle);
        }
        else if (_currentFigure is RectangleFigure rectangle)
        {
            DrawRectangle(rectangle);
        }
    }

    private void DrawTriangle(Triangle triangle)
    {
        DrawLine(triangle.P1, triangle.P2);
        DrawLine(triangle.P2, triangle.P3);
        DrawLine(triangle.P3, triangle.P1);
    }

    private void DrawRectangle(RectangleFigure rectangle)
    {
        DrawLine(rectangle.P1, rectangle.P2);
        DrawLine(rectangle.P2, rectangle.P3);
        DrawLine(rectangle.P3, rectangle.P4);
        DrawLine(rectangle.P4, rectangle.P1);
    }

    private void DrawLine(Point2D p1, Point2D p2)
    {
        var line = new Line
        {
            Stroke = Brushes.IndianRed,
            StrokeThickness = 3,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round,
            X1 = p1.X,
            Y1 = p1.Y,
            X2 = p2.X,
            Y2 = p2.Y
        };

        Scene.Children.Add(line);
    }
}
