using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osi.Core;
using System.Text;
using System.Threading.Tasks;

namespace osi.Desktop
{
	public class DownloaderHelper
	{

		public class Downloader : BackgroundWorker
		{
			private WebClient webClient;

			public Downloader() : base()
			{
				WorkerReportsProgress = true;
				WorkerSupportsCancellation = true;

				DoWork += Downloader_DoWork;
			}

			private void Downloader_DoWork(object sender, DoWorkEventArgs e)
			{
				BeatmapsetDownloadListItemViewModel item = (BeatmapsetDownloadListItemViewModel)e.Argument;

				string fileName = $"{item.BeatmapsetId} {item.Artist}-{item.Title}.osz";
				string path = $"{Environment.CurrentDirectory}/download/{fileName}";

				webClient = new WebClient();
				webClient.DownloadProgressChanged += (s, a) => {
					if (CancellationPending == true)
					{
						((WebClient)s).CancelAsync();
						e.Cancel = true;
					}
				};

				webClient.DownloadFileCompleted += (b, r) =>
				{
					e.Result = path;
				};
				
				webClient.DownloadFileAsync(new Uri($"https://osu.sayobot.cn/osu.php?s={item.BeatmapsetId}"), path);
			}
		}
	}
}
