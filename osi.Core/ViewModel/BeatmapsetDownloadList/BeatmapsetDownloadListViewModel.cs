using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core
{
	public class BeatmapsetDownloadListViewModel:BaseViewModel
	{
		public ObservableCollection<BeatmapsetDownloadListItemViewModel> Items { get; set; } = new ObservableCollection<BeatmapsetDownloadListItemViewModel>();
	}
}
