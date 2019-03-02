
namespace osi.Core
{
	public class DownloadMessageDesignModel : DownloadMessageViewModel
	{

		#region Singleton

		public static DownloadMessageViewModel Instance => new DownloadMessageDesignModel();

		#endregion

		#region  Constructor
		public DownloadMessageDesignModel()
		{
			BeatmapsetId = 923990;
			BPM = 218;
			Title = "White parade";
			Artist = "Umeboshi Chazuke";
			Creator = "ATing";
			Progress = 0.50f;
			Length = 196;
			DownloadStatus = Status.Error;
		}

		#endregion



	}
}
