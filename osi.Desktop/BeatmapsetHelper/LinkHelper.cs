using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace osi.Desktop
{
	public static class LinkHelper
	{
		public class NotValidUri : Exception
		{
			public Uri Uri;

			public NotValidUri(Uri uri) : base("Not a valid Uri")
			{
				this.Data.Add(("Uri"), uri);
			}
		}
		public static int ToBeatmapsetId(Uri uri)
		{
			int id = -1;

			string Host = uri.Host.ToLower();
			if (Host == "osu.ppy.sh")
			{
				Regex osuRegex = new Regex(@"^/(?<type>[bsdp])/(beatmap\?b\=)?(?<id>\d+)");
				Regex osuNewRegex = new Regex(@"^/beatmapsets/(?<sid>\d+)(#(?<mode>\w+)/(?<bid>\d+))?");

				if (osuRegex.IsMatch((uri.PathAndQuery)))
				{
					Match value = osuRegex.Match(uri.PathAndQuery);
					switch (value.Groups["type"].Value)
					{
						case "b":
						case "p":
							//Beatmap Id
							id = int.Parse(value.Groups["id"].Value);
							id = ToBeatmapsetId(id);
							break;
						case "s":
						case "d":
							//Beatmapset Id
							id = int.Parse(value.Groups["id"].Value);
							break;
					}
				}
				else if (osuNewRegex.IsMatch(uri.PathAndQuery))
				{
					Match value = osuNewRegex.Match(uri.PathAndQuery);
					if (value.Groups["sid"].Value != "")
					//永远为 true Mark
					{
						//Beatmapset Id
						id = int.Parse(value.Groups["sid"].Value);
					}
					else
					{
						//Beatmap Id
						id = int.Parse(value.Groups["bid"].Value);
					}
				}
			}
			else
			{
				throw new NotValidUri(uri);
			}

			return id;
		}
		public static int ToBeatmapsetId(int beatmapId)
		{
			WebClient client = new WebClient();
			client.Method = "Head";
			client.AllowAutoRedirect = false;
			client.DownloadString($"http://osu.ppy.sh/b/{beatmapId}");
			return ToBeatmapsetId(new Uri(client.ResponseHeaders["Location"]));
		}
	}
}
