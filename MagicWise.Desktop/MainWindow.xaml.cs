using MagicWise.Desktop.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MagicWise.Desktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is MainViewModel oldVm)
        {
            oldVm.PropertyChanged -= OnViewModelPropertyChanged;
        }

        if (e.NewValue is MainViewModel newVm)
        {
            newVm.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.IsPanelOpen))
        {
            AnimatePanel((DataContext as MainViewModel)!.IsPanelOpen);
        }
    }

    private void AnimatePanel(bool open)
    {
        double targetWidth = open ? 300 : 0;
        var animation = new GridLengthAnimation
        {
            From = LeftPanelColumn.Width,
            To = new GridLength(targetWidth),
            Duration = new Duration(TimeSpan.FromMilliseconds(220)),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
        };
        LeftPanelColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
    }
}
