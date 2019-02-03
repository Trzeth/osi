using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Desktop.Helper
{
	public static class AnalyticsModel
	{
		public static class Category
		{
			public const string Application = "Application";
			public const string User = "User";
		}
		public static class Action
		{
			public const string Startup = "Startup";
			public const string Exit = "Exit";
			public const string Update = "Update";
			public const string Install = "Install";


			public const string DownloadBeatmapset = "DownloadBeatmapset";
		}
		public static class Lable
		{
			public const string Succeed = "Succeed";
			public const string Failed = "Failed";

			public static string BeatmapsetId(int id)
			{
				return id.ToString();
			}
		}
	}
}
