using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace osi.Desktop
{
	public class BeatmapsetIdToImageConverter : BaseValueConverter<BeatmapsetIdToImageConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int BeatmapsetId = int.Parse(value.ToString());
			BitmapImage bitmapImage;
			try
			{
				bitmapImage = new BitmapImage(new Uri($"https://cdn.sayobot.cn:25225/thumb/{BeatmapsetId}l.jpg"));
			}
			catch(Exception e)
			{
				//BUGGGY
				bitmapImage = new BitmapImage(new Uri("pack://Application;,,,/Image/File.png"));
			}
			return bitmapImage;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
