using osi.Core.DownloadManager;

namespace osi.Core
{
 	public class DownloadStatusViewModel:BaseViewModel
	{
		public DownloadingStatus DownloadingStatus { get; set; }

		public float Progress { get; set; }

	}
}
