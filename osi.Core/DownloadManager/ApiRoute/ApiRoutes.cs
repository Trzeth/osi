using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Core.DownloadManager.ApiRoute
{
	public static class ApiRoutes
	{
		public static class Api
		{
			public const string Host = "https://api.sayobot.cn/";

			public const string BeatmapList = "beatmaplist";

			public const string BeatmapInfo = "v2/beatmapinfo";

			public const string PackList = "packlist";

			public const string Notice = "static/notice";

			public const string New = "static/news";

			public const string Servers = "static/servers";

			public const string Support = "static/support";

			public const string SupportList = "static/supportlist";
		}
		public static class Resources
		{
			public const string PreviewThumb = "https://cdn.sayobot.cn:25225/beatmaps/{0}/covers/cover.jpg";

			public const string PreviewThumbSquare = "https://cdn.sayobot.cn:25225/thumb/{0}l.jpg";

			public const string PreviewSound = "https://cdnx.sayobot.cn:25225/preview/{0}.mp3";

			public const string FullSound = "https://txy1.sayobot.cn/maps/audio/{0}";

			public static class Beatmapsets
			{
				public const string Beatmapset = "https://txy1.sayobot.cn/download/osz/{0}";

				public const string BeatmapsetNoVideo = "https://txy1.sayobot.cn/download/osz/novideo/{0}";

				public const string BeatmapsetBackup = "https://osu.sayobot.cn/osu.php?s={0}";
			}
		}
	}
}
