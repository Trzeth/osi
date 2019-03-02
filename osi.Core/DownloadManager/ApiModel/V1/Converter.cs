using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel.V1
{
	public static class Converter
	{
		public static List<BeatmapsetInformation> ToBeatmapInformationList(this BeatmapList list)
		{
			List<BeatmapsetInformation> information = new List<BeatmapsetInformation>();

			foreach(BeatmapListItem item in list.data)
			{
				information.Add(item.ToBeatmapInformation());
			}

			return information;
		}

		public static BeatmapsetInformation ToBeatmapInformation(this BeatmapListItem item)
		{
			BeatmapsetInformation i = new BeatmapsetInformation();
			i.BeatmapsetId = item.sid;
			i.RankStatus = item.approved.ToRankStatus();
			i.Artist = item.artist;
			i.Creator = item.creator;
			i.FavouriteCount = item.favourite_count;
			i.LastUpdate = ApiModel.Converter.TimeStampToDateTime(item.lastupdate);
		    i.Modes = ApiModel.Converter.IntToModes(item.modes);
			i.PlayCount = item.order;
			i.Title = item.title;

			return i;
		}
	}
}
