using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace VkEasyPhones.Converters
{
	public class enTypeConverter<T> : IValueConverter where T : struct, IConvertible
	{
		#region IValueConverter Members

		public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

			T type = (T)value;
			T converted = default(T);
			if (Enum.TryParse<T>(parameter as string, out converted))
			{
				return type.Equals(converted);
			}
			return false;
		}

		public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

			T res = default(T);

			Enum.TryParse<T>(parameter as string, out res);

			return res;
		}

		#endregion
	}
}
