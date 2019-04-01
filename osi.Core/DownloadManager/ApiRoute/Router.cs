using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				public static string GetBeatmapListString(ListType type, int limit = 25, int offset = 0)
				{
					if (type == ListType.Search)
						Debugger.Break();

					return GetBeatmapListString(type, null, null, limit, offset);
				}
				public static string GetBeatmapListString(ListType type, BeatmapsetFilter filter = null,object keyword = null,int limit = 25, int offset = 0)
				{
					string s = GetAbsoluteRoute(ApiRoutes.Api.BeatmapList);
					s += $"?0={limit}";
					s += $"&1={offset}";
					s += "&2=";
					switch (type) {
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
					if (type == ListType.Search)
					{
						if(keyword != null)
						{
							s += $"&3={keyword}";
						}
						if(filter != null)
						{
							s += $"&4={GetSum(filter.SubTypes)}";
							s += $"&5={GetSum(filter.Modes)}";
							s += $"&6={GetSum(filter.RankStatus)}";
							s += $"&7={GetSum(filter.Genres)}";
							s += $"&8={GetSum(filter.Languages)}";
							s += $"&7={filter.Other}";
						}
					}
					return s;
				}
				public static int GetSum<T>(List<T> list)
				{
					if (!typeof(T).IsEnum)
					{
						Debugger.Break();
					}

					int i = 0;
					foreach(T t in list)
					{
						i += (int)Enum.Parse(typeof(T),t.ToString());
					}
					return i;
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
