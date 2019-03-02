using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel.V1
{
	public class BeatmapListItem
	{
		public int sid { get; set; }

		public int modes { get; set; }

		public RankStatus approved { get; set; }

		public long lastupdate { get; set; }

		public string title { get; set; }

		public string titleU { get; set; }

		public string artist { get; set; }

		public string artistU { get; set; }

		public string creator { get; set; }

		public int favourite_count { get; set; }

		public int order { get; set; }

	}
}
