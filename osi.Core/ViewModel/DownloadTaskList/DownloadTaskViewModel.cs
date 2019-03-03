namespace osi.Core
{
	public class DownloadTaskViewModel:BeatmapsetInformation
	{
		public DownloadStatusViewModel DownloadStatus { get; set; }

		public RelayCommand RetryCommand { get;set; }

		public DownloadTaskViewModel()
		{
			RetryCommand = new RelayCommand(() =>
			{
				DownloadManager.DownloadManager.Current.RetryDownloadBeatmapset(this);
			}
			);
		}

		public DownloadTaskViewModel(int beatmapsetId)
		{
			BeatmapsetId = beatmapsetId;
			

			RetryCommand = new RelayCommand(()=>
			{
				DownloadManager.DownloadManager.Current.RetryDownloadBeatmapset(this);
			}
			);
		}
	}
}
