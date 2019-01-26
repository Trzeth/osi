using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
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
			bitmapImage = new BitmapImage(new Uri($"https://cdn.sayobot.cn:25225/thumb/{BeatmapsetId}l.jpg"));
			//WebClient webClient = new WebClient();

			//try
			//{
			//	byte[] image = webClient.DownloadData($"https://cdn.sayobot.cn:25225/thumb/{BeatmapsetId}l.jpg");
			//	bitmapImage = new BitmapImage();
			//	bitmapImage.BeginInit();
			//	bitmapImage.StreamSource = new MemoryStream(image);
			//	bitmapImage.EndInit();
			//}
			//catch(Exception e)
			//{
			//	Debugger.Log(0,"Expectoin",e.Message);
			//	bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Image/File.png", UriKind.Absolute));
			//}

			return bitmapImage;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
