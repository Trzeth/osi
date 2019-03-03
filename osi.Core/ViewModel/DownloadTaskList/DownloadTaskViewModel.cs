namespace osi.Core
{
	public class DownloadTaskViewModel:BeatmapsetInformation
	{
		public DownloadStatusViewModel DownloadStatus { get; set; }

		public DownloadTaskViewModel() { }

		public DownloadTaskViewModel(int beatmapsetId)
		{
			BeatmapsetId = beatmapsetId;
		}
	}
}
