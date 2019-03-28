using GoogleAnalyticsTracker.Simple;
using osi.Core;
using osi.Desktop.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi.Desktop
{
    public class LinkMonitor
    {
		public static LinkMonitor Current = new LinkMonitor();

		private Core.DownloadManager.DownloadManager DownloadManager = Core.DownloadManager.DownloadManager.Current;
		private string mPreviousLink;

		public async Task StartMointorAsync()
		{
			NamedPipeServerStream server = new NamedPipeServerStream("osi", PipeDirection.In);
			string link = null;
			while (true)
			{
				server.WaitForConnection();

				StreamReader sr = new StreamReader(server);
				link = sr.ReadToEnd();
				server.Disconnect();

				if (link == "Stop")
					return;

				Uri uri = null;

				try
				{
					uri = new Uri(link);
					int beatmapsetId = LinkHelper.ToBeatmapsetId(uri);
					if (link == mPreviousLink)
					{
						Config.Current.Registry.UserBrowserRegistry.OpenUrl(uri);
					}
					else
					{
						DownloadManager.DownloadBeatmapsetAsync(beatmapsetId);

						AnalyticsHelper.Current.TrackEventAsync(AnalyticsModel.Category.User, AnalyticsModel.Action.DownloadBeatmapset, beatmapsetId.ToString(), null);
						mPreviousLink = link;
					}
				}
				catch (LinkHelper.NotValidUri)
				{
					Config.Current.Registry.UserBrowserRegistry.OpenUrl(uri);
				}
				catch (UriFormatException) { }
			}

		}
		public void StopMonitor()
		{
			NamedPipeClientStream client = new NamedPipeClientStream(".", "osi", PipeDirection.Out);
			client.Connect();
			StreamWriter sw = new StreamWriter(client);
			sw.Write("Stop");
			sw.Flush();
			sw.Close();
			client.Close();
		}
	}
}
