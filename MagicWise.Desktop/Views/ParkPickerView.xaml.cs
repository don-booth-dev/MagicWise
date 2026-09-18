using MagicWise.Desktop.ViewModels;
using System.Windows.Controls;

namespace MagicWise.Desktop.Views;

public partial class ParkPickerView : UserControl
{
    public ParkPickerView()
    {
        InitializeComponent();
    }

    private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
    {
        if (DataContext is ParkPickerViewModel vm && e.NewValue is ParkViewModel park)
        {
            vm.SelectPark(park);
        }
    }
}
