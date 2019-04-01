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

		public string SearchBarText { get; set; }
	}
}
