using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel.V2
{
	public class BeatmapInfo
	{
		public int status { get; set; }

		public Beatmapset data { get; set; }
	}
}
