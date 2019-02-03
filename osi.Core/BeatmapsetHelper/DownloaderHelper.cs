using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osi.Core;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace osi.Core
{
	public class DownloaderHelper
	{

		public class Downloader : WebClient
		{
			#region Public Members

			public BeatmapsetDownloadListItemViewModel BeatmapsetDownloadListItemViewModel { get; set; }

			public EventHandler DownloadBeatmapsetCompleted;
			#endregion

			#region Constructor

			public Downloader() : base()
			{

			}

			public Downloader(BeatmapsetDownloadListItemViewModel item) : base()
			{
				BeatmapsetDownloadListItemViewModel = item;
			}

			#endregion

			protected override void OnDownloadFileCompleted(AsyncCompletedEventArgs e)
			{
				if (DownloadBeatmapsetCompleted != null) DownloadBeatmapsetCompleted(this,e);

				base.OnDownloadFileCompleted(e);
			}
			public async Task DownloadBeatmapset()
			{
				if (BeatmapsetDownloadListItemViewModel == null) return;
				BeatmapsetDownloadListItemViewModel item = BeatmapsetDownloadListItemViewModel;

				string fileName = $"{item.BeatmapsetId} {item.Artist}-{item.Title}.osz";
				string path = $"{Environment.CurrentDirectory}/download/{fileName}";
				this.DownloadFileAsync(new Uri($"https://osu.sayobot.cn/osu.php?s={item.BeatmapsetId}"), path);
			}
		}
	}
}
