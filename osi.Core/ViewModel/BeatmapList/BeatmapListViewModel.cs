using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core
{
	public class BeatmapListViewModel:BaseViewModel
	{
		public List<BeatmapListItemViewModel> Items { get; set; } = new List<BeatmapListItemViewModel>();
	}
}
