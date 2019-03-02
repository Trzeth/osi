using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel.V1
{
	public class BeatmapList
	{
		public int status { get; set; }

		public int endid { get; set; }

		public int time_cost { get; set; }

		public int results { get; set; }

		// 以下仅在 endid = 0 时出现
		public int match_title_results { get; set; }

		public int match_artist_results { get; set; }

		public int match_creator_results { get; set; }

		public int match_version_results { get; set; }

		public int match_tags_results { get; set; }

		public List<BeatmapListItem> data { get; set; }
	}
}
