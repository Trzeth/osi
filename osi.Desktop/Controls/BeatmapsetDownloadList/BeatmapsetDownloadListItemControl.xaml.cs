using Newtonsoft.Json.Linq;
using osi.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace osi.Desktop
{
	/// <summary>
	/// BeatmapListControl.xaml 的交互逻辑
	/// </summary>
	public partial class BeatmapsetDownloadListItemControl : UserControl
	{
		BeatmapsetDownloadListItemViewModel item;
		
		DownloaderHelper.Downloader downloader;

		public BeatmapsetDownloadListItemControl()
		{
			InitializeComponent();

			DataContextChanged += BeatmapsetDownloadListItemControl_DataContextChanged;

		}

		private void BeatmapsetDownloadListItemControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			DataContextChanged -= BeatmapsetDownloadListItemControl_DataContextChanged;

			if (DataContext != null) item = (BeatmapsetDownloadListItemViewModel)DataContext;
			DataContext = item = GetBeatmapsetInformataion(item);

			downloader = new DownloaderHelper.Downloader();
			downloader.ProgressChanged += (s, r) =>
			{
				((BeatmapsetDownloadListItemViewModel)DataContext).ProgressPercentage = r.ProgressPercentage;
			};

			downloader.RunWorkerAsync(item);
		}

		private BeatmapsetDownloadListItemViewModel GetBeatmapsetInformataion(BeatmapsetDownloadListItemViewModel item)
		{
			int beatmapsetId = item.BeatmapsetId;
			WebClient webClient = new WebClient();

			JObject data = JObject.Parse(webClient.DownloadString($"https://api.sayobot.cn/v2/beatmapinfo?0={beatmapsetId}"));
			JToken beatmapset = data["data"];

			BeatmapsetDownloadListItemViewModel newItem = new BeatmapsetDownloadListItemViewModel();

			if (beatmapsetId != int.Parse(beatmapset["sid"].ToString()))
			{
				throw new Exception("Not Found");
			}

			newItem.BeatmapsetId = int.Parse(beatmapset["sid"].ToString());
			newItem.Artist = (string)beatmapset["artist"];
			newItem.BeatmapsetId = int.Parse(beatmapset["sid"].ToString());
			newItem.Title = (string)beatmapset["title"];
			newItem.Creator = (string)beatmapset["creator"];

			return newItem;
		}
	}
}
