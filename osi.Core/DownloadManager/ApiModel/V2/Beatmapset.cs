using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel.V2
{
	public class Beatmapset
	{
		public int sid { get; set; }

		public long local_update { get; set; }

		public int bids_amount { get; set; }

		public RankStatus approved { get; set; }

		public string title { get; set; }

		public string titleU { get; set; }

		public string artist { get; set; }

		public string artistU { get; set; }

		public string creator { get; set; }

		public int creator_id { get; set; }

		public string source { get; set; }

		public long last_update { get; set; }

		/// <summary>
		/// 从第一个 Beatmap 中取出的不准确的时间		
		/// </summary>
		public int length
		{
			get { return bid_data.First().length; }
		}

		public long approved_date { get; set; }

		public int favourite_count { get; set; }

		public int video{ get; set; }

		public int storyboard { get; set; }

		public int preview { get; set; }

		public string tags { get; set; }

		public Language language { get; set; }

		public Genre genre { get; set; }

		public List<Beatmap> bid_data { get; set; }
	}
}
