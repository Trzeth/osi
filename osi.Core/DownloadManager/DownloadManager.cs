using osi.Core.DownloadManager.ApiRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager
{
	public class DownloadManager
	{

		#region Public Properties

		public static DownloadManager Current = new DownloadManager();

		public EventHandler<DownloadCompleteEventArgs> DownloadComplete;

		public class DownloadCompleteEventArgs : EventArgs
		{
			public string Path { get; set; }
		}

		public void OnDownloadComplete(DownloadCompleteEventArgs e)
		{
			DownloadComplete?.Invoke(this, e);
		}

		public string Path { get; set; } = @"E:\Coding\osi\osi.Desktop\bin\Debug\download\123.osz";


		#endregion

		#region Private Properties

		private AsyncObservableCollection<DownloadTaskViewModel> mDownloadTaskList
		{
			get { return IoC.Get<DownloadTaskListViewModel>().Items; }
		}

		#endregion

		#region Contructor

		public DownloadManager(){ } 

		#endregion


		public async Task DownloadBeatmapsetAsync(int beatmapsetId)
		{
			DownloadBeatmapsetAsync(new DownloadTaskViewModel(beatmapsetId));
		}

		public async Task DownloadBeatmapsetAsync(DownloadTaskViewModel downloadTaskViewModel)
		{
			await downloadTaskViewModel.GetInformationAsync();
			mDownloadTaskList.Add(downloadTaskViewModel);

			Download(downloadTaskViewModel.BeatmapsetId, downloadTaskViewModel);
		}

		public void RetryDownloadBeatmapset(DownloadTaskViewModel downloadTaskViewModel)
		{
			//downloadTaskViewModel.DownloadStatus.DownloadingStatus = DownloadingStatus.Downloading;

			//Task.Run(() => Download(downloadTaskViewModel.BeatmapsetId, downloadTaskViewModel.DownloadStatus));

		}

		private void Download(int beatmapsetId, DownloadTaskViewModel downloadTaskViewModel)
		{
			WebClient webClient = new WebClient();

			downloadTaskViewModel.DownloadingStatus = DownloadingStatus.Downloading;
			webClient.DownloadProgressChanged += (sender, e) =>
			{
				downloadTaskViewModel.Progress = ((float)e.ProgressPercentage) / 100;
			};

			webClient.DownloadFileCompleted += (sender, e) =>
			{
				if (e.Error != null)
				{

				}
				else
				{
					downloadTaskViewModel.DownloadingStatus = DownloadingStatus.Complete;

					OnDownloadComplete(new DownloadCompleteEventArgs() { Path = this.Path });
				}
			};
			Uri s = Router.Resource.Beatmapsets.GetBeatmapsetUri(beatmapsetId);
			webClient.DownloadFileAsync(s, Path);
		}
	}
}
