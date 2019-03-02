using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Desktop
{
	public class SecondsToTimeSpanConverter : BaseValueConverter<SecondsToTimeSpanConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int i = int.Parse(value.ToString());
			return TimeSpan.FromSeconds(i);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
