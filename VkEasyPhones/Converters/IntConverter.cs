using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace VkEasyPhones.Converters
{
	public class IntConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((int)value).ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int res = 0;
			if (int.TryParse(value as string, out res))
			{
				return res;
			}
			return res;
		}
	}
}
