using System.Windows;

namespace GeometryDrawing.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ClearScene_Click(object sender, RoutedEventArgs e)
    {
        Scene.Children.Clear();
        StatusText.Text = "Холст очищен.";
    }
}

