using System;

namespace osi.Core
{
	public class DownloadTaskListDesignModel : DownloadTaskListViewModel
	{

		#region Singleton

		public static DownloadTaskListDesignModel Instance => new DownloadTaskListDesignModel();

		#endregion

		#region  Constructor

		public DownloadTaskListDesignModel():base()
		{
			Items = new System.Collections.ObjectModel.ObservableCollection<DownloadTaskViewModel>()
			{
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 923990,
					BPM = 218,
					Title = "White parade",
					Artist = "Umeboshi Chazuke",
					Creator = "ATing",
					Length = TimeSpan.FromSeconds(196),

					DownloadStatus = new DownloadStatusViewModel()
					{
						DownloadingStatus = DownloadManager.DownloadingStatus.Downloading,
						Progress = 0.5f
					},
				},
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 923990,
					BPM = 218,
					Title = "White parade",
					Artist = "Umeboshi Chazuke",
					Creator = "ATing",
					Length = TimeSpan.FromSeconds(196),

					DownloadStatus = new DownloadStatusViewModel()
					{
						DownloadingStatus = DownloadManager.DownloadingStatus.Downloading,
						Progress = 0.5f
					},
				},
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 923990,
					BPM = 218,
					Title = "White parade",
					Artist = "Umeboshi Chazuke",
					Creator = "ATing",
					Length = TimeSpan.FromSeconds(196),

					DownloadStatus = new DownloadStatusViewModel()
					{
						DownloadingStatus = DownloadManager.DownloadingStatus.Downloading,
						Progress = 0.5f
					},
				}
			};
		}

		#endregion



	}
}
