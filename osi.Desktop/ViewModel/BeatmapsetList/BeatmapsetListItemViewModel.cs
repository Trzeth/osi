using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace osi.Desktop
{
	public class BeatmapsetListItemViewModel
	{

		public int OuterBorderPadding { get; set; } = 5;

		public Thickness OuterBorderPaddingThickness { get { return new Thickness(OuterBorderPadding); }}

		public int BeatmapsetId { get; set; } = 372271;

		public float BPM { get; set; }

		public string Title { get; set; }

		public string Creator { get; set; }

		public string Artist { get; set; }
		  
		public string[] Tags { get; set; }

		public string Source { get; set; }

	}
}
