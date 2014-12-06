using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using VKSyncMusic.Handlers.Synchronize;

namespace VKSyncMusic.Converters
{
    public class StateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valueConverted = (SyncStates)value;
            var paramConverted = (SyncStates)Enum.Parse(typeof(SyncStates), parameter.ToString() , true); 

            if (valueConverted == SyncStates.Default)
                return Visibility.Collapsed;

            if (paramConverted == valueConverted)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
