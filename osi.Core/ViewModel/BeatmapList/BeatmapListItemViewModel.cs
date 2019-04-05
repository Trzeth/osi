using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osi.Core.DownloadManager;

namespace osi.Core
{
	public class BeatmapListItemViewModel:BaseViewModel
	{
		public BeatmapsetInformation BeatmapsetInformation { get; set; }

		public RelayCommand ClickCommand { get; set; }

		public bool HasGetDetailed { get; set; } = false;

		public BeatmapListItemViewModel()
		{
			ClickCommand = new RelayCommand(() => 
			{
				//DownloadManager.DownloadManager.Current.DownloadBeatmapsetAsync(BeatmapsetId);
			});
		}
	}
}
