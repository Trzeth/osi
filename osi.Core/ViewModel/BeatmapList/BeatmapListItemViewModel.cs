using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osi.Core.DownloadManager;

namespace osi.Core
{
	public class BeatmapListItemViewModel:BeatmapsetInformation
	{
		public RelayCommand ClickCommand { get; set; }
		public BeatmapListItemViewModel()
		{
			ClickCommand = new RelayCommand(() => 
			{
				//DownloadManager.DownloadManager.Current.DownloadBeatmapsetAsync(BeatmapsetId);
			});
		}
	}
}
