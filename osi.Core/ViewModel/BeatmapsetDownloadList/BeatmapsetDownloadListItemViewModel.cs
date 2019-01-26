using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace osi.Core
{
	public class BeatmapsetDownloadListItemViewModel:BeatmapsetListItemViewModel
	{
		public int ProgressPercentage;
		public float Progress
		{
			get { return ProgressPercentage / 100; }
		}
	}
}
