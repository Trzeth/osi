using Newtonsoft.Json.Linq;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace osi.Core
{

	public class BeatmapsetDownloadListItemViewModel:BeatmapsetListItemViewModel
	{
		#region Private Members

		private int mRetryCount = 0;

		#endregion

		#region Pulic Member
		public float Progress { get; set; }

		public DownloadStatus DownloadStatus { get; set; }

		public int MaxRetryCount { get; set; } = 3;

		public int BeatmapsetId { get; set; }
		#endregion

		#region  Constructor
		public BeatmapsetDownloadListItemViewModel() : base() { }
		public BeatmapsetDownloadListItemViewModel(int BeatmapsetId) : base()
		{
			this.BeatmapsetId = BeatmapsetId;
			GetBeatmapsetInformation();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Get BeatmapsetInformation from sayobot
		/// </summary>
		/// <returns></returns>
		public void GetBeatmapsetInformation()
		{
			Task.Run(GetBeatmapsetInformationAsync);
		}

		/// <summary>
		/// Get BeatmapsetInformation from sayobot Async
		/// </summary>
		/// <returns></returns>
		public async Task GetBeatmapsetInformationAsync()
		{
			int beatmapsetId = BeatmapsetId;

			if (beatmapsetId == 0)
				return;

			JObject data = JObject.Parse(new Core.WebClient().DownloadString($"https://api.sayobot.cn/v2/beatmapinfo?0={beatmapsetId}"));


			JToken beatmapset = data["data"];

			if (beatmapsetId != int.Parse(beatmapset["sid"].ToString()))
			{
				throw new Exception("Not Found");
			}

			BeatmapsetId = beatmapsetId;
			Artist = (string)beatmapset["artist"];
			Title = (string)beatmapset["title"];
			Creator = (string)beatmapset["creator"];
			ThumbSource = $"https://cdn.sayobot.cn:25225/thumb/{beatmapsetId}l.jpg";

		}

		/// <summary>
		/// Download Beatmapset
		/// </summary>
		public void DownloadBeatmapset()
		{
			Task.Run(DownloadBeatmapsetAsync);
		}

		/// <summary>
		/// Download Beatmapset Async
		/// </summary>
		public async Task DownloadBeatmapsetAsync()
		{
			DownloadStatus = Status.Downloading;

			DownloaderHelper.Downloader downloader = new DownloaderHelper.Downloader(this);

			downloader.DownloadProgressChanged += (sender, e) =>
			{
				Progress =  ((float)e.ProgressPercentage)/100;
			};

			downloader.DownloadFileCompleted += (sender, e) =>
			{
				if(e.Error!=null)
				{
					mRetryCount++;
					if (mRetryCount <= MaxRetryCount)
					{
						Task.Run(DownloadBeatmapsetAsync);
					}
					else
					{
						DownloadStatus = Status.Error;
					}
				}
				else
				{
					DownloadStatus = Status.Finished;

					Process.Start(((DownloaderHelper.Downloader)sender).FilePath);
				}
			};
			await downloader.DownloadBeatmapset();
		}

		public void Download()
		{
			Task.Run(() =>
			{
				Task.Run(GetBeatmapsetInformationAsync).Wait();
				
				Task.Run(DownloadBeatmapsetAsync);


			});
		}
		#endregion


	}
}
