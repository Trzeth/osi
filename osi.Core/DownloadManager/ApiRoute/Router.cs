using System;
using System.Collections.Generic;
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
			/// GetBeatmapInfo string
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
				public enum UriKind
				{
					New,
					Hot,
					Search
				}

				//TODO YOU SHOULD FINISH IT
				public static string GetBeatmapListString()
				{
					string s = GetAbsoluteRoute(ApiRoutes.Api.BeatmapList);

					return s;
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
