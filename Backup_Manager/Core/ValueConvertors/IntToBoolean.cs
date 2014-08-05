using System;
using System.Windows.Data;

namespace Backup_Manager.Core.ValueConvertors
{
    public class IntToBoolean : IValueConverter
    {
        public IntToBoolean Instance = new IntToBoolean();

        public object Convert(object values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.ToString() == "0")
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "1";
            else
                return "0";
        }
    }
}