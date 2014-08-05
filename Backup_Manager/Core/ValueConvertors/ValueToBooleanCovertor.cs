using System;
using System.Windows.Data;

namespace Backup_Manager.Core.ValueConvertors
{
    /// <summary>
    /// Used for enabling and disabling controls via XAML code.
    /// Created and developed by Omar Bizreh.
    /// </summary>
    internal class ValueToBooleanConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            foreach (object value in values)
            {
                if ((bool)value)
                {
                    return true;
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}