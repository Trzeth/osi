using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace osi.Core
{

	public class BeatmapsetListItemViewModel:BaseViewModel
	{
		#region Private Members

		private BitmapImage mImage;

		#endregion

		#region Public Members
		public int BeatmapsetId { get; set; }

		public float BPM { get; set; }

		public string Title { get; set; }

		public string Creator { get; set; }

		public string Artist { get; set; }

		public string[] Tags { get; set; }

		public string Source { get; set; }

		public string ThumbSource { get; set; }

		//public BitmapImage Image {
		//	get
		//	{
		//		if (mImage != null) return mImage;

		//		Task.Run(() =>
		//		{
		//			BitmapImage bitmapImage;
		//			try
		//			{
		//				byte[] image = new WebClient().DownloadData($"https://cdn.sayobot.cn:25225/thumb/{BeatmapsetId}l.jpg");
		//				bitmapImage = new BitmapImage();
		//				bitmapImage.BeginInit();
		//				bitmapImage.StreamSource = new MemoryStream(image);
		//				bitmapImage.EndInit();
		//			}
		//			catch (Exception e)
		//			{
		//				Debugger.Log(0, "Expectoin", e.Message);
		//				bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Image/File.png", UriKind.Absolute));
		//			}

		//			return bitmapImage;
		//		});
		//		return;
		//	}
		//	set
		//	{
		//		mImage = value;
		//	}
		//}

		#endregion

	}
}
