using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core
{
	public class BeatmapListViewModel:BeatmapInformation
	{
		public List<BeatmapListItemViewModel> Items { get; set; } = new List<BeatmapListItemViewModel>();
	}
}
