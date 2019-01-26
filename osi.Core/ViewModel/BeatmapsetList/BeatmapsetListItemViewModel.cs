using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace osi.Core
{
	public class BeatmapsetListItemViewModel:BaseViewModel
	{
		public int BeatmapsetId { get; set; }

		public float BPM { get; set; }

		public string Title { get; set; }

		public string Creator { get; set; }

		public string Artist { get; set; }
		  
		public string[] Tags { get; set; }

		public string Source { get; set; }

	}
}
