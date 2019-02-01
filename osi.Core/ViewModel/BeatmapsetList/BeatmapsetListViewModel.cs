using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core
{
	public class BeatmapsetListViewModel:BaseViewModel
	{
		public List<BeatmapsetListItemViewModel> Items { get; set; } = new List<BeatmapsetListItemViewModel>();
	}
}
