
using System;

namespace osi.Core
{
	public class DownloadTaskDesignModel : DownloadTaskViewModel
	{

		#region Singleton

		public static DownloadTaskDesignModel Instance => new DownloadTaskDesignModel();

		#endregion

		#region  Constructor

		public DownloadTaskDesignModel()
		{
			BeatmapsetId = 923990;
			BPM = 218;
			Title = "White parade";
			Artist = "Umeboshi Chazuke";
			Creator = "ATing";
			Length = TimeSpan.FromSeconds(196);

			DownloadStatus = new DownloadStatusViewModel()
			{
				DownloadingStatus = DownloadManager.DownloadingStatus.Downloading,
				Progress = 0.5f
			};
		}

		#endregion



	}
}
