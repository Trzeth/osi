
using System;
using System.Collections.Generic;

namespace osi.Core
{
	public enum RankStatus
	{
		Approved,
		Qualified,
		Loved,
		Ranked,
		Pending,
		Graveyard,
		WIP
	}

	public enum Mode
	{
		Standard,
		Taiko,
		Catch_the_Beat,
		Mania
	}

	public enum Genre
	{
		Any,
		Unspecified,
		Video_Game,
		Anime,
		Rock,
		Pop,
		Other,
		Novelty,
		Hip_Hop,
		Electronic
	}

	public enum Language
	{
		Any,
		English,
		Chinese,
		French,
		German,
		Italian,
		Japanese,
		Korean,
		Spanish,
		Swedish,
		Instrumental,
		Other
	}

	public class BeatmapsetInformation:BaseViewModel
	{
		private string mTagsString;

		public int BeatmapsetId { get; set; }

		public DateTime UpdateTime { get; set; }

		public RankStatus RankStatus { get; set; }

		public string Title { get; set; }

		public string Artist { get; set; }

		public string Creator { get; set; }

		public int CreatorId { get; set; }

		public string Source { get; set; }

		public DateTime LastUpdate { get; set; }

		public DateTime ApprovedDate { get; set; }

		public float BPM { get; set; }

		public int FavouriteCount { get; set; }

		public int PlayCount { get; set; }

		/// <summary>
		/// 0 = false 1 = true
		/// </summary>
		public bool HasVideo { get; set; }

		public bool HasStoryboard { get; set; }

		//用途不明 TODO
		public bool HasPreview { get; set; }

		/// <summary>
		/// Tags List
		/// </summary>
		public List<string> Tags{ get; set; }

		/// <summary>
		/// Tags String
		/// </summary>
		public string TagsString
		{
			get { return mTagsString; }
			set
			{
				Tags = new List<string>();
				Tags.AddRange(value.Split(' '));
				mTagsString = value;
			}
		}

		public Language Language { get; set; }

		public Genre Genre { get; set; }

		public string ThumbSource { get; set; }

		public TimeSpan Length { get; set; }

		public List<Mode> Modes { get; set; }

		public List<BeatmapInformation> Beatmaps { get; set; }
	}
}
