using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel.V2
{
	public static class Converter
	{
		public static void ToBeatmapsetInformation(this BeatmapInfo info,ref BeatmapsetInformation sI)
		{
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
			sI.Length = TimeSpan.FromSeconds(beatmapset.length);
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

			List<BeatmapInformation> bI = new List<BeatmapInformation>();
			foreach (Beatmap b in beatmapset.bid_data)
			{
				bI.Add(b.ToBeatmapInformation());
			}
			sI.Beatmaps = bI;
		}

		private static BeatmapInformation ToBeatmapInformation(this Beatmap beatmap)
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
	}
}
