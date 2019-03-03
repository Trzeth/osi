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
					BPM = 214,
					Title = "Call of Abyss",
					Artist = "Atusu",
					Creator = "Cyberspace-",
					Length = TimeSpan.FromSeconds(214),

					DownloadStatus = new DownloadStatusViewModel()
					{
						DownloadingStatus = DownloadManager.DownloadingStatus.Error,
						Progress = 0f
					},
				},
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 923990,
					BPM = 200,
					Title = "Sigmund",
					Artist = "Gram",
					Creator = "-[Apple]-",
					Length = TimeSpan.FromSeconds(196),

					DownloadStatus = new DownloadStatusViewModel()
					{
						DownloadingStatus = DownloadManager.DownloadingStatus.Complete,
						Progress = 1f
					},
				},
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 923990,
					BPM = 200,
					Title = "Sigmund",
					Artist = "Gram",
					Creator = "-[Apple]-",
					Length = TimeSpan.FromSeconds(196),

					DownloadStatus = new DownloadStatusViewModel()
					{
						DownloadingStatus = DownloadManager.DownloadingStatus.Complete,
						Progress = 1f
					},
				}
			};
		}

		#endregion



	}
}
