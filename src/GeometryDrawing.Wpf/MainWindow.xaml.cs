using System.Windows;
using System.Windows.Controls;
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
    private Brush _lineBrush = Brushes.IndianRed;
    private double _strokeThickness = 3;

    public MainWindow()
    {
        InitializeComponent();
        LineColorComboBox.SelectionChanged += FigureStyle_Changed;
        StrokeThicknessSlider.ValueChanged += FigureStyle_Changed;
    }

    private void FigureStyle_Changed(object sender, RoutedEventArgs e)
    {
        if (LineColorComboBox.SelectedItem is ComboBoxItem { Tag: string colorName })
        {
            _lineBrush = colorName switch
            {
                "RoyalBlue" => Brushes.RoyalBlue,
                "SeaGreen" => Brushes.SeaGreen,
                "DarkViolet" => Brushes.DarkViolet,
                "Black" => Brushes.Black,
                _ => Brushes.IndianRed
            };
        }

        _strokeThickness = StrokeThicknessSlider.Value;
        DrawCurrentFigure();

        if (_currentFigure is not null)
        {
            StatusText.Text = $"Стиль обновлён: толщина {_strokeThickness:0}.";
        }
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

    private void ManualTriangle_Click(object sender, RoutedEventArgs e)
    {
        if (!TryReadPoint(P1XTextBox, P1YTextBox, "P1", out Point2D p1)
            || !TryReadPoint(P2XTextBox, P2YTextBox, "P2", out Point2D p2)
            || !TryReadPoint(P3XTextBox, P3YTextBox, "P3", out Point2D p3))
        {
            return;
        }

        try
        {
            var triangle = new Triangle(p1, p2, p3);
            if (!IsInsideScene(triangle))
            {
                ShowValidationError("Все точки должны находиться внутри холста 680 × 520.");
                return;
            }

            _currentFigure = triangle;
            DrawCurrentFigure();
            StatusText.Text = "Создан треугольник по заданным точкам.";
        }
        catch (ArgumentException exception)
        {
            ShowValidationError(exception.Message);
        }
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

    private void ManualRectangle_Click(object sender, RoutedEventArgs e)
    {
        CreateManualRectangle(isSquare: false);
    }

    private void ManualSquare_Click(object sender, RoutedEventArgs e)
    {
        CreateManualRectangle(isSquare: true);
    }

    private void RandomSquare_Click(object sender, RoutedEventArgs e)
    {
        int side = _random.Next(70, 230);
        int x = _random.Next(ScenePadding, SceneWidth - ScenePadding - side);
        int y = _random.Next(ScenePadding, SceneHeight - ScenePadding - side);

        _currentFigure = RectangleFigure.CreateSquare(new Point2D(x, y), side);
        DrawCurrentFigure();
        StatusText.Text = $"Создан квадрат со стороной {side}.";
    }

    private void MoveFigure_Click(object sender, RoutedEventArgs e)
    {
        if (!TryReadInteger(MoveXTextBox, "ΔX", out int deltaX)
            || !TryReadInteger(MoveYTextBox, "ΔY", out int deltaY))
        {
            return;
        }

        MoveCurrentFigure(deltaX, deltaY);
    }

    private void NudgeFigure_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: string movement })
        {
            return;
        }

        string[] parts = movement.Split(';');
        if (parts.Length == 2
            && int.TryParse(parts[0], out int deltaX)
            && int.TryParse(parts[1], out int deltaY))
        {
            MoveCurrentFigure(deltaX, deltaY);
        }
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

    private void CreateManualRectangle(bool isSquare)
    {
        if (!TryReadInteger(RectangleXTextBox, "X", out int x)
            || !TryReadInteger(RectangleYTextBox, "Y", out int y))
        {
            return;
        }

        int width;
        int height;
        if (isSquare)
        {
            if (!TryReadPositiveInteger(SquareSideTextBox, "Сторона квадрата", out int side))
            {
                return;
            }

            width = side;
            height = side;
        }
        else
        {
            if (!TryReadPositiveInteger(RectangleWidthTextBox, "Ширина", out width)
                || !TryReadPositiveInteger(RectangleHeightTextBox, "Высота", out height))
            {
                return;
            }
        }

        try
        {
            var rectangle = new RectangleFigure(new Point2D(x, y), width, height);
            if (!IsInsideScene(rectangle))
            {
                ShowValidationError("Фигура должна полностью находиться внутри холста 680 × 520.");
                return;
            }

            _currentFigure = rectangle;
            DrawCurrentFigure();
            StatusText.Text = isSquare
                ? $"Создан квадрат со стороной {width}."
                : $"Создан прямоугольник {width} × {height}.";
        }
        catch (ArgumentException exception)
        {
            ShowValidationError(exception.Message);
        }
        catch (OverflowException)
        {
            ShowValidationError("Введённые значения слишком велики.");
        }
    }

    private void MoveCurrentFigure(int deltaX, int deltaY)
    {
        if (_currentFigure is null)
        {
            ShowValidationError("Сначала создайте фигуру.");
            return;
        }

        if (!FigureMovement.TryMoveWithin(
                _currentFigure,
                deltaX,
                deltaY,
                SceneWidth,
                SceneHeight))
        {
            ShowValidationError("После такого перемещения фигура выйдет за границы холста.");
            return;
        }

        DrawCurrentFigure();
        StatusText.Text = $"Фигура перемещена: ΔX = {deltaX}, ΔY = {deltaY}.";
    }

    private bool TryReadPoint(
        TextBox xTextBox,
        TextBox yTextBox,
        string pointName,
        out Point2D point)
    {
        point = null!;
        if (!TryReadInteger(xTextBox, $"{pointName}.X", out int x)
            || !TryReadInteger(yTextBox, $"{pointName}.Y", out int y))
        {
            return false;
        }

        point = new Point2D(x, y);
        return true;
    }

    private bool TryReadPositiveInteger(TextBox textBox, string fieldName, out int value)
    {
        if (!TryReadInteger(textBox, fieldName, out value))
        {
            return false;
        }

        if (value <= 0)
        {
            ShowValidationError($"Поле «{fieldName}» должно быть больше нуля.");
            textBox.Focus();
            return false;
        }

        return true;
    }

    private bool TryReadInteger(TextBox textBox, string fieldName, out int value)
    {
        if (int.TryParse(textBox.Text, out value))
        {
            return true;
        }

        ShowValidationError($"Поле «{fieldName}» должно содержать целое число.");
        textBox.Focus();
        textBox.SelectAll();
        return false;
    }

    private static bool IsInsideScene(IFigure figure)
    {
        return figure.Points.All(point =>
            point.X >= 0 && point.X <= SceneWidth
            && point.Y >= 0 && point.Y <= SceneHeight);
    }

    private void ShowValidationError(string message)
    {
        StatusText.Text = message;
        MessageBox.Show(this, message, "Проверьте данные", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            Stroke = _lineBrush,
            StrokeThickness = _strokeThickness,
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
