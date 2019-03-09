using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace osi.Desktop
{
	public class RectangleConverter : BaseValueConverter<RectangleConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string[] parameters = (parameter as string).Split('|');
			return new Rect(0,0,double.Parse(parameters[0]),double.Parse(parameters[1]));
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
