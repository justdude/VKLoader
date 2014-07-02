using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace VKMusicSync.Converters
{

    public class VisibilityToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var visibility = (bool)value;
            var param = parameter as string;
            var target = Visibility.Hidden;
            if (param == "Collapsed")
                target = Visibility.Collapsed;

            if (visibility)
            {
                if (param == "Invert")
                    return Visibility.Hidden;
                if (param == "InvertCollapsed")
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }

            if (param == "Invert")
                return Visibility.Visible;
            if (param == "InvertCollapsed")
                return Visibility.Collapsed;

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            var res = false;

            if (visibility == Visibility.Collapsed || visibility == Visibility.Hidden)
            {
                res = true;
            }

            string param = parameter as string;
            if (param == "Invert")
                res = !res;

            return res;
            throw new NotImplementedException();
        }
    }
}
