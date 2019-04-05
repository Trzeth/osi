using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SmartEnum;

namespace osi.Core.DownloadManager.ApiRoute
{
	public static class Router
	{
		public static class Api
		{
			public static string GetAbsoluteRoute(string relativeUrl)
			{
				return ApiRoutes.Api.Host + relativeUrl;
			}

			/// <summary>
			/// Get BeatmapInfoApi Path string
			/// </summary>
			/// <param name="keyword">关键词</param>
			/// <param name="isForceBeatmapId">是否强制匹配BeatmapId</param>
			/// <returns></returns>
			public static string GetBeatmapInfoString(object keyword,bool isForceBeatmapId = false)
			{
				string s = GetAbsoluteRoute(ApiRoutes.Api.BeatmapInfo);
				if (keyword != null) s += $"?0={keyword.ToString()}";
				if (isForceBeatmapId) s += $"&1=1";

				return s;
			}

			public static class BeatmapList
			{
				//TODO YOU SHOULD FINISH IT
				public static string GetBeatmapListString(BeatmapsetFilter filter)
				{
					string s = GetAbsoluteRoute(ApiRoutes.Api.BeatmapList);
					s += $"?0={filter.Range.Length()}";
					s += $"&1={filter.Range.From}";
					s += "&2=";

					switch (filter.ListType)
					{
						case ListType.Hot:
							s += "1";
							break;
						case ListType.New:
							s += "2";
							break;
						case ListType.Package:
							s += "3";
							break;
						case ListType.Search:
							s += "4";
							break;
					}
					if (filter.ListType == ListType.Search)
					{
						if (filter.SearchString != null)
						{
							s += $"&3={filter.SearchString}";
						}
						if (filter.ExtendFilter != null)
						{
							ExtendFilter extendFilter = filter.ExtendFilter;
							s += $"&4={GetSum(extendFilter.SubTypes)}";
							s += $"&5={GetSum(extendFilter.Modes)}";
							s += $"&6={GetSum(extendFilter.RankStatus)}";
							s += $"&7={GetSum(extendFilter.Genres)}";
							s += $"&8={GetSum(extendFilter.Languages)}";
							s += $"&7={extendFilter.Other}";
						}
					}
					return s;
				}

				public static int GetSum<T>(List<T> list) where T : SmartEnum<T>
				{
					int sum = 0;
					foreach(T t in list)
					{
						sum += t.Value;
					}
					return sum;
				}
			}
		}

		public static class Resource
		{
			public static class Beatmapsets
			{
				public enum UriKind
				{
					Beatmapset,
					NoVideo,
					Backup
				}

				public static Uri GetBeatmapsetUri(int beatmapsetId, UriKind uriKind = UriKind.Beatmapset)
				{
					return new Uri(GetBeatmapsetString(beatmapsetId,uriKind));
				}
				public static string GetBeatmapsetString(int beatmapsetId,UriKind uriKind = UriKind.Beatmapset)
				{
					string baseFormat = null;
					switch (uriKind)
					{
						case UriKind.Beatmapset:
							baseFormat = ApiRoutes.Resources.Beatmapsets.Beatmapset;
							break;
						case UriKind.NoVideo:
							baseFormat = ApiRoutes.Resources.Beatmapsets.BeatmapsetNoVideo;
							break;
						case UriKind.Backup:
							baseFormat = ApiRoutes.Resources.Beatmapsets.BeatmapsetBackup;
							break;
					}

					return string.Format(baseFormat, beatmapsetId);
				}
			}
		}
	}
}
