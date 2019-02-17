using Newtonsoft.Json.Linq;
using osi.Core;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace osi.Desktop
{
	public class BeatmapsetInformationConverter: BaseValueConverter<BeatmapsetInformationConverter>
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			BeatmapsetListItemViewModel viewModel = new BeatmapsetListItemViewModel();
			int beatmapsetId = (int)value;
			
			if (beatmapsetId == 0)
				return null;

			JObject data = JObject.Parse(new Core.WebClient().DownloadString($"https://api.sayobot.cn/v2/beatmapinfo?0={beatmapsetId}"));


			JToken beatmapset = data["data"];

			if (beatmapsetId != int.Parse(beatmapset["sid"].ToString()))
			{
				throw new Exception("Not Found");
			}

			viewModel.BeatmapsetId = beatmapsetId;
			viewModel.Artist = (string)beatmapset["artist"];
			viewModel.Title = (string)beatmapset["title"];
			viewModel.Creator = (string)beatmapset["creator"];
			viewModel.ThumbSource = $"https://cdn.sayobot.cn:25225/thumb/{beatmapsetId}l.jpg";

			return viewModel;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
