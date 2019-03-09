using System;

namespace osi.Core
{
	public class DownloadTaskListDesignModel : DownloadTaskListViewModel
	{

		#region Singleton

		public static DownloadTaskListDesignModel Instance => new DownloadTaskListDesignModel();

		#endregion

		#region  Constructor

		public DownloadTaskListDesignModel()
		{
			Items = new AsyncObservableCollection<DownloadTaskViewModel>()
			{
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 923990,
					BPM = 218,
					Title = "White parade",
					Artist = "Umeboshi Chazuke",
					Creator = "ATing",
					Length = TimeSpan.FromSeconds(196),

					DownloadingStatus = DownloadManager.DownloadingStatus.Downloading,
					Progress = 0.5f
				},
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 910644,
					BPM = 214,
					Title = "Call of Abyss",
					Artist = "Atusu",
					Creator = "Cyberspace-",
					Length = TimeSpan.FromSeconds(214),

					DownloadingStatus = DownloadManager.DownloadingStatus.Error,
					Progress = 0f
				},
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 915444,
					BPM = 200,
					Title = "Sigmund",
					Artist = "Gram",
					Creator = "-[Apple]-",
					Length = TimeSpan.FromSeconds(196),

					DownloadingStatus = DownloadManager.DownloadingStatus.Complete,
					Progress = 1f
				},
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 921696,
					BPM = 190,
					Title = "Vous etes fatigues",
					Artist = "Billx & Strez",
					Creator = "[JOS]losffa",
					Length = TimeSpan.FromSeconds(196),

					DownloadingStatus = DownloadManager.DownloadingStatus.Complete,
					Progress = 1f
				},
				new DownloadTaskViewModel()
				{
					BeatmapsetId = 934371,
					BPM = 190,
					Title = "killy killy JOKER",
					Artist = "Wakeshima Kanon",
					Creator = "shuniki",
					Length = TimeSpan.FromSeconds(196),

					DownloadingStatus = DownloadManager.DownloadingStatus.Downloading,
					Progress = 0.7f
				},
			};

		}

		#endregion



	}
}
