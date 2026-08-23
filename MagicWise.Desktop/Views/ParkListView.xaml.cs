using MagicWise.Desktop.ViewModels;
using System.Windows.Controls;

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
}
