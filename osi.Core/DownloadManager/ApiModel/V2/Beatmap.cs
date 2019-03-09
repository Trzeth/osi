using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel.V2
{
	public class Beatmap
	{
		public int bid { get; set; }

		public string version { get; set; }

		public int length { get; set; }

		public float cs { get; set; }

		public float ar { get; set; }

		public float od { get; set; }

		public float hp { get; set; }

		public float star { get; set; }

		public float aim { get; set; }

		public int circles { get; set; }

		public int sliders { get; set; }

		public int spinners { get; set; }

		public int maxcombo { get; set; }

		public Mode mode { get; set; }

		public int playcount { get; set; }

		public int passcount { get; set; }

		public string curve { get; set; }

		public string img { get; set; }
	}
}
