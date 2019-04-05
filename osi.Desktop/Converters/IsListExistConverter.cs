using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Ardalis.SmartEnum;

namespace osi.Desktop
{
	public class IsListExistConverter : BaseValueConverter<IsListExistConverter>
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
