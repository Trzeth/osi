using osi.Core.DownloadManager.ApiRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager
{
	public class DownloadManager
	{

		#region Public Properties

		public EventHandler<DownloadCompleteEventArgs> DownloadComplete;

		public class DownloadCompleteEventArgs : EventArgs
		{
			public string Path { get; set; }
		}

		public void OnDownloadComplete(DownloadCompleteEventArgs e)
		{
			DownloadComplete(this, e);
		}

		public string Path { get; set; }


		#endregion

		#region Private Properties

		private static DownloadTaskListViewModel mDownloadTaskListViewModel { get; set; }

		#endregion

		#region Contructor

		public DownloadManager()
		{
			mDownloadTaskListViewModel = IoC.Get<DownloadTaskListViewModel>();
		} 

		#endregion


		public DownloadStatusViewModel DownloadBeatmapset(int beatmapsetId)
		{
			return DownloadBeatmapset(new DownloadTaskViewModel(beatmapsetId));
		}

		public DownloadStatusViewModel DownloadBeatmapset(DownloadTaskViewModel downloadTaskViewModel)
		{
			DownloadStatusViewModel downloadStatusViewModel = new DownloadStatusViewModel();
			downloadTaskViewModel.DownloadStatus = downloadStatusViewModel;
			mDownloadTaskListViewModel.Items.Add(downloadTaskViewModel);

			Task.Run(() => Download(downloadTaskViewModel.BeatmapsetId,downloadStatusViewModel));

			return downloadStatusViewModel;
		}

		private void Download(int beatmapsetId,DownloadStatusViewModel downloadStatusViewModel)
		{
			WebClient webClient = new WebClient();

			webClient.DownloadProgressChanged += (sender, e) =>
			{
				downloadStatusViewModel.Progress = ((float)e.ProgressPercentage) / 100;
			};

			webClient.DownloadFileCompleted += (sender, e) =>
			{
				if (e.Error != null)
				{

				}
				else
				{
					downloadStatusViewModel.DownloadingStatus = DownloadingStatus.Complete;

					OnDownloadComplete(new DownloadCompleteEventArgs() { Path = this.Path });
				}
			};

			webClient.DownloadFileAsync(Router.Resource.Beatmapsets.GetBeatmapsetUri(beatmapsetId), Path);
		}
	}
}
