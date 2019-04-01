using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager
{
	public enum ListType
	{
		New,
		Hot,
		Package,
		Search
	}
	public enum SubType
	{
		Title = 1,
		Artist = 2,
		Creator = 4,
		Version = 8,
		Tags = 16,
		Source = 32
	}
	public enum Mode
	{
		Standard = 1,
		Taiko = 2,
		Catch_the_Beat = 4,
		Mania = 8
	}
	public enum RankStatus
	{
		Approved_And_Ranked = 1,
		Qualified = 2,
		Loved = 4,
		Pending_And_WIP = 8,
		Graveyard = 16
	}
	public enum Genre
	{
		Any = 1,
		Unspecified = 2,
		Video_Game = 4,
		Anime = 8,
		Rock = 16,
		Pop = 32,
		Other = 64,
		Novelty = 128,
		Hip_Hop = 256,
		Electronic = 1024
	}

	public enum Language
	{
		Any = 1,
		Other = 2,
		English = 4,
		Japanese = 8,
		Chinese = 16,
		Instrumental = 32,
		Korean = 64,
		French = 128,
		German = 256,
		Swedish = 512,
		Spanish = 1024,
		Italian = 2048,
	}
	public class Other
	{
		public Range Star;
		public Range AR;
		public Range OD;
		public Range CS;
		public Range HP;
		public Range Length;
		public Range BPM;
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append('"');
			sb.Append($"star:{Star.From}~{Star.To}");
			sb.Append($",AR:{AR.From}~{AR.To}");
			sb.Append($",OD:{OD.From}~{OD.To}");
			sb.Append($",CS:{CS.From}~{CS.To}");
			sb.Append($",HP:{HP.From}~{HP.To}");
			sb.Append($",length:{Length.From}~{Length.To}");
			sb.Append($",BPM:{BPM.From}~{BPM.To}");
			sb.Append(",end");
			sb.Append('"');

			return sb.ToString();
		}
	}
	public struct Range
	{
		public int From;
		public int To;
		public Range(int From,int To)
		{
			this.From = From;
			this.To = To;
		}
	}

	public class BeatmapsetFilter
	{
		public List<SubType> SubTypes;
		public List<Mode> Modes;
		public List<RankStatus> RankStatus;
		public List<Genre> Genres;
		public List<Language> Languages;
		public Other Other;
	}
}
