using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using VkEasyPhones.Enumartions;

namespace VkEasyPhones.Converters
{
	public class enPeoplesTypeConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			enPeoplesTypes type = (enPeoplesTypes)value;
			enPeoplesTypes converted = enPeoplesTypes.None;
			if (Enum.TryParse<enPeoplesTypes>(parameter as string, out converted))
			{
				return type == converted;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			enPeoplesTypes res = enPeoplesTypes.None;

			Enum.TryParse<enPeoplesTypes>(parameter as string, out res);

			return res;
		}

		#endregion
	}
}
