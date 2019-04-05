using osi.Core.DownloadManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class BeatmapListPageViewModel : BaseViewModel
    {
		public BeatmapListViewModel BeatmapListViewModel { get; set; }

		public string SearchBarString { get; set; }

		public BeatmapsetFilter CurrentBeatmapsetFilter { get; set; }

		public ExtendFilter DefaultExtendFilterFilter { get; set; }

		public List<ExtendFilter> ExtendFilterFilters { get; set; }
	}
}
