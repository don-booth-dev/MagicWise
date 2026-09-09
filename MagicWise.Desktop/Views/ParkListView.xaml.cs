using MagicWise.Desktop.ViewModels;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MagicWise.Desktop.Views;

public partial class ParkListView : UserControl
{
    public ParkListView()
    {
        InitializeComponent();
    }

    private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
    {
        if (DataContext is MainViewModel vm && e.NewValue is ParkViewModel park)
        {
            vm.SelectedPark = park;
        }
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = e.Uri.AbsoluteUri,
            UseShellExecute = true
        });
        e.Handled = true;
    }
}
