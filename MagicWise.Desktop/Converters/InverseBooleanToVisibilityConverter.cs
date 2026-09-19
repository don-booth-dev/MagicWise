using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MagicWise.Desktop.Converters;

/// <summary>
/// Like <see cref="System.Windows.Controls.BooleanToVisibilityConverter"/> but
/// inverted: true collapses, false shows. Used for empty-state hints that
/// should disappear once a condition becomes true.
/// </summary>
public class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool boolValue = value is bool b && b;
        return boolValue ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isVisible = value is Visibility v && v == Visibility.Visible;
        return !isVisible;
    }
}
