using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SmartEnum;

namespace osi.Core.DownloadManager
{
	public enum ListType
	{
		New,
		Hot,
		Package,
		Search
	}
	public sealed class SubType : SmartEnum<SubType>
	{
		public static readonly SubType Title = new SubType(nameof(Title),1);
		public static readonly SubType Artist = new SubType(nameof(Artist),2);
		public static readonly SubType Creator = new SubType(nameof(Creator),4);
		public static readonly SubType Version = new SubType(nameof(Version),8);
		public static readonly SubType Tags = new SubType(nameof(Tags),16);
		public static readonly SubType Source = new SubType(nameof(Source),32);

		private SubType(string s, int i) : base(s, i) { }
	}
	//public enum SubType
	//{
	//	Title = 1,
	//	Artist = 2,
	//	Creator = 4,
	//	Version = 8,
	//	Tags = 16,
	//	Source = 32
	//}
	public sealed class Mode : SmartEnum<Mode>
	{
		public static readonly Mode Standard = new Mode(nameof(Standard),1);
		public static readonly Mode Taiko = new Mode(nameof(Taiko),2);
		public static readonly Mode Catch_the_Beat = new Mode(nameof(Catch_the_Beat),4);
		public static readonly Mode Mania = new Mode(nameof(Mania),8);

		private Mode(string s, int i) : base(s, i) { }
	}
	//public enum Mode
	//{
	//	Standard = 1,
	//	Taiko = 2,
	//	Catch_the_Beat = 4,
	//	Mania = 8
	//}

	public sealed class RankStatus : SmartEnum<RankStatus>
	{
		public static readonly RankStatus Approved_And_Ranked = new RankStatus(nameof(Approved_And_Ranked), 1);
		public static readonly RankStatus Qualified = new RankStatus(nameof(Qualified), 2);
		public static readonly RankStatus Loved = new RankStatus(nameof(Loved), 4);
		public static readonly RankStatus Pending_And_WIP = new RankStatus(nameof(Pending_And_WIP), 8);
		public static readonly RankStatus Graveyard = new RankStatus(nameof(Graveyard), 16);

		private RankStatus(string s, int i) : base(s, i) { }

	}
	//public enum RankStatus
	//{
	//	Approved_And_Ranked = 1,
	//	Qualified = 2,
	//	Loved = 4,
	//	Pending_And_WIP = 8,
	//	Graveyard = 16
	//}
	public sealed class Genre : SmartEnum<Genre>
	{
		public static readonly Genre Any = new Genre(nameof(Any), 1);
		public static readonly Genre Unspecified = new Genre(nameof(Unspecified), 2);
		public static readonly Genre Video_Game = new Genre(nameof(Video_Game), 4);
		public static readonly Genre Anime = new Genre(nameof(Anime), 8);
		public static readonly Genre Rock = new Genre(nameof(Rock), 16);
		public static readonly Genre Pop = new Genre(nameof(Pop), 32);
		public static readonly Genre Other = new Genre(nameof(Other), 64);
		public static readonly Genre Novelty = new Genre(nameof(Novelty), 128);
		public static readonly Genre Hip_Hop = new Genre(nameof(Hip_Hop), 256);
		public static readonly Genre Electronic = new Genre(nameof(Electronic), 1024);

		private Genre(string s, int i) : base(s, i) { }
	}
	//public enum Genre
	//{
	//	Any = 1,
	//	Unspecified = 2,
	//	Video_Game = 4,
	//	Anime = 8,
	//	Rock = 16,
	//	Pop = 32,
	//	Other = 64,
	//	Novelty = 128,
	//	Hip_Hop = 256,
	//	Electronic = 1024
	//}
	public sealed class Language : SmartEnum<Language>
	{
		public static readonly Language Any = new Language(nameof(Any), 1);
		public static readonly Language Other = new Language(nameof(Other), 2);
		public static readonly Language English = new Language(nameof(English), 4);
		public static readonly Language Japanese = new Language(nameof(Japanese), 8);
		public static readonly Language Chinese = new Language(nameof(Chinese), 16);
		public static readonly Language Instrumental = new Language(nameof(Instrumental), 32);
		public static readonly Language Korean = new Language(nameof(Korean), 64);
		public static readonly Language French = new Language(nameof(French), 128);
		public static readonly Language German = new Language(nameof(German), 256);
		public static readonly Language Swedish = new Language(nameof(Swedish), 512);
		public static readonly Language Spanish = new Language(nameof(Spanish), 1024);
		public static readonly Language Italian = new Language(nameof(Italian), 2048);

		private Language(string s, int i) : base(s, i) { }
	}
	//public enum Language
	//{
	//	Any = 1,
	//	Other = 2,
	//	English = 4,
	//	Japanese = 8,
	//	Chinese = 16,
	//	Instrumental = 32,
	//	Korean = 64,
	//	French = 128,
	//	German = 256,
	//	Swedish = 512,
	//	Spanish = 1024,
	//	Italian = 2048,
	//}
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
			if (To < From)
			{
				this.From = 0;
				this.To = 0;
			}
			else
			{
				this.From = From;
				this.To = To;
			}
		}

		public int Length()
		{
			return To - From;
		}
		/// <summary>
		/// 将 From 和 To 都增加 i
		/// </summary>
		/// <param name="i"></param>
		public Range AddToBoth(int i)
		{
			From += i;
			To += i;

			return this;
		}
	}

	public class BeatmapsetFilter
	{
		public ExtendFilter ExtendFilter { get; set; }
		public ListType ListType { get; set; }
		public string SearchString { get; set; }
		public Range Range { get; set; }

		public BeatmapsetFilter() { ExtendFilter = null; }
		public BeatmapsetFilter(ListType type, Range range)
		{
			if(type == ListType.Search)
				throw new Exception("只有搜索时才能有关键词");

			ListType = type;
			Range = range;
			ExtendFilter = null;
		}
		public BeatmapsetFilter(ListType type, string s, Range range)
		{
			if (type != ListType.Search)
				throw new Exception("只有搜索时才能有关键词");

			ListType = type;
			SearchString = s;
			Range = range;
			ExtendFilter = null;
		}
		public BeatmapsetFilter(string s, Range range)
		{
			ListType = ListType.Search;
			SearchString = s;
			Range = range;
			ExtendFilter = null;
		}
		public BeatmapsetFilter(ListType type, string s, Range range,ExtendFilter extendFilter)
		{
			ListType = type;
			SearchString = s;
			Range = range;
			ExtendFilter = extendFilter;
		}
	}

	public class ExtendFilter
	{
		public List<SubType> SubTypes { get; set; }
		public List<Mode> Modes { get; set; }
		public List<RankStatus> RankStatus { get; set; }
		public List<Genre> Genres { get; set; }
		public List<Language> Languages { get; set; }
		public Other Other { get; set; }
	}
}
