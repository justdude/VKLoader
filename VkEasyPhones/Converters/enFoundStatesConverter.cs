using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkEasyPhones.Enumartions;

namespace VkEasyPhones.Converters
{
	public class enFoundStatesConverter : enTypeConverter<enFoundStates>
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return base.Convert(value, targetType, parameter, culture);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return base.ConvertBack(value, targetType, parameter, culture);
		}
	}
}
