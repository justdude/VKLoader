using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using VKMusicSync.Model;

namespace VKMusicSync.Converters
{
    public class ImagePathConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var uri = value as ImageModel;
            if (uri == null)
                return null;

           // var xmlRes = new BitmapImage( new Uri(uri.ImagePath));
           // return xmlRes;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
