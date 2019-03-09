
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

			DownloadingStatus = DownloadManager.DownloadingStatus.Downloading;
			Progress = 1f;
		}

		#endregion



	}
}
