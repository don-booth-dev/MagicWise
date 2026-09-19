using MagicWise.Desktop.ViewModels;
using System.Windows.Controls;

namespace MagicWise.Desktop.Views;

public partial class ResortPickerView : UserControl
{
    public ResortPickerView()
    {
        InitializeComponent();
    }

    private void DestinationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is ResortPickerViewModel vm && e.AddedItems.Count > 0 && e.AddedItems[0] is DestinationViewModel destination)
        {
            vm.SelectDestination(destination);
        }
    }
}
