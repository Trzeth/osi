using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel.V2
{
	public static class Converter
	{
		public static BeatmapsetInformation ToBeatmapInformation(this BeatmapInfo info)
		{
			BeatmapsetInformation sI = new BeatmapsetInformation();

			Beatmapset beatmapset = info.data;

			sI.RankStatus = beatmapset.approved.ToRankStatus();
			sI.ApprovedDate = ApiModel.Converter.TimeStampToDateTime(beatmapset.approved_date);
			sI.Artist = beatmapset.artist;
			//beatmapset.artistU;
			//beatmapset.bids_amount;
			sI.Creator = beatmapset.creator;
			sI.CreatorId = beatmapset.creator_id;
			sI.FavouriteCount = beatmapset.favourite_count;
			sI.Genre = beatmapset.genre.ToGenre();
			sI.Language = beatmapset.language.ToLanguage();
			sI.LastUpdate = ApiModel.Converter.TimeStampToDateTime(beatmapset.last_update);
			sI.UpdateTime = ApiModel.Converter.TimeStampToDateTime(beatmapset.local_update);
			sI.HasPreview = ApiModel.Converter.IntToBoolen(beatmapset.preview);
			sI.BeatmapsetId = beatmapset.sid;
			sI.Source = beatmapset.source;
		    sI.HasStoryboard = ApiModel.Converter.IntToBoolen(beatmapset.storyboard);
			sI.TagsString = beatmapset.tags;
			sI.Title = beatmapset.title;
			//beatmapset.titleU;
			sI.HasVideo = ApiModel.Converter.IntToBoolen(beatmapset.video);

			foreach (Beatmap b in beatmapset.bid_data)
			{
				List<BeatmapInformation> bI = new List<BeatmapInformation>();
				bI.Add(b.ToBeatmapInformation());
			}

			return sI;
		}

		public static BeatmapInformation ToBeatmapInformation(this Beatmap beatmap)
		{
			BeatmapInformation bI = new BeatmapInformation();
			//beatmap.aim;
			bI.ApproachRate = beatmap.ar;
			bI.BeatmapId = beatmap.bid;
			bI.CircleCount = beatmap.circles;
			bI.CircleSize = beatmap.cs;
			//beatmap.curve;
			bI.HPDrain = beatmap.hp;

			//beatmap.img;

			bI.Length = beatmap.length;
			bI.MaxCombo = beatmap.maxcombo;
			bI.Mode = beatmap.mode.ToMode();
			bI.OverallDifficulty = beatmap.od;
			bI.PassCount = beatmap.passcount;
			bI.PlayCount = beatmap.playcount;
			bI.SlidersCount = beatmap.sliders;
			bI.SpinnersCount = beatmap.spinners;
			bI.Star = beatmap.star;
			bI.Version = beatmap.version;

			return bI;
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
		    i.Modes = item.modes.ToModes();
			i.PlayCount = item.order;
			i.Title = item.title;

			return i;
		}
		private static Core.RankStatus ToRankStatus(this RankStatus status)
		{
			switch (status)
			{
				case RankStatus.approved:
					return Core.RankStatus.Approved;
				case RankStatus.graveyard:
					return Core.RankStatus.Graveyard;
				case RankStatus.loved:
					return Core.RankStatus.Loved;
				case RankStatus.pending:
					return Core.RankStatus.Pending;
				case RankStatus.qualified:
					return Core.RankStatus.Qualified;
				case RankStatus.ranked:
					return Core.RankStatus.Ranked;
				case RankStatus.WIP:
					return Core.RankStatus.WIP;
				default:
					return new Core.RankStatus();
			}
		}

		private static List<Core.Mode> ToModes(this int i)
		{
			List<Core.Mode> modes = new List<Core.Mode>();
			
			if(i%2 == 1)
			{
				modes.Add(Core.Mode.Standard);
				i -= 1;
			}
			if(i%8 >= 1)
			{
				modes.Add(Core.Mode.Mania);
				i -= 8;
			}
			if(i%4 >= 1)
			{
				modes.Add(Core.Mode.Catch_the_Beat);
				i -= 4;
			}
			if(i%2 >= 1)
			{
				modes.Add(Core.Mode.Taiko);
				i -= 2;
			}

			return modes;
		}
	}
}
