using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiModel
{
	public static class Converter
	{
		public static DateTime TimeStampToDateTime(long l)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(l);
		}

		public static Core.RankStatus ToRankStatus(this RankStatus status)
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

		public static Core.Genre ToGenre(this Genre genre)
		{
			switch (genre)
			{
				case Genre.anime:
					return Core.Genre.Anime;
				case Genre.any:
					return Core.Genre.Any;
				case Genre.electronic:
					return Core.Genre.Electronic;
				case Genre.hip_hop:
					return Core.Genre.Hip_Hop;
				case Genre.novelty:
					return Core.Genre.Novelty;
				case Genre.other:
					return Core.Genre.Other;
				case Genre.pop:
					return Core.Genre.Pop;
				case Genre.rock:
					return Core.Genre.Rock;
				case Genre.unspecified:
					return Core.Genre.Unspecified;
				case Genre.video_game:
					return Core.Genre.Video_Game;
				default:
					return new Core.Genre();
			}
		}

		public static Core.Language ToLanguage(this Language language)
		{
			switch (language)
			{
				case Language.any:
					return Core.Language.Any;
				case Language.chinese:
					return Core.Language.Chinese;
				case Language.english:
					return Core.Language.English;
				case Language.french:
					return Core.Language.French;
				case Language.german:
					return Core.Language.German;
				case Language.instrumental:
					return Core.Language.Instrumental;
				case Language.italian:
					return Core.Language.Italian;
				case Language.japanese:
					return Core.Language.Japanese;
				case Language.korean:
					return Core.Language.Korean;
				case Language.other:
					return Core.Language.Other;
				case Language.spanish:
					return Core.Language.Spanish;
				case Language.swedish:
					return Core.Language.Swedish;
				default:
					return new Core.Language();
			}
		}

		public static Core.Mode ToMode(this Mode mode)
		{
			switch (mode)
			{
				case Mode.ctb:
					return Core.Mode.Catch_the_Beat;
				case Mode.mania:
					return Core.Mode.Mania;
				case Mode.osu:
					return Core.Mode.Standard;
				case Mode.taiko:
					return Core.Mode.Taiko;
				default:
					return new Core.Mode();
			}
		}

		public static List<Core.Mode> IntToModes(int i)
		{
			List<Core.Mode> modes = new List<Core.Mode>();

			if (i % 2 == 1)
			{
				modes.Add(Core.Mode.Standard);
				i -= 1;
			}
			if (i % 8 >= 1)
			{
				modes.Add(Core.Mode.Mania);
				i -= 8;
			}
			if (i % 4 >= 1)
			{
				modes.Add(Core.Mode.Catch_the_Beat);
				i -= 4;
			}
			if (i % 2 >= 1)
			{
				modes.Add(Core.Mode.Taiko);
				i -= 2;
			}

			return modes;
		}

		public static bool IntToBoolen(int i)
		{
			return i == 0 ? false : true;
		}
	}
}
