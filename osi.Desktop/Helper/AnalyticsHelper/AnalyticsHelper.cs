using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GoogleAnalyticsTracker.Core.TrackerParameters.Interface;
using GoogleAnalyticsTracker.Simple;
using GoogleAnalyticsTracker.Core;

namespace osi.Desktop.Helper
{
	public class AnalyticsHelper : SimpleTracker
	{

		private const string Account = "UA-121062181-1";
		private const string IpApi = @"https://api.ip.sb/ip";
		private static string Ip = new WebClient().DownloadString(IpApi);

		//Guid = UserId per app
		private static Guid Guid;
		//ClientId = UserName per player
		private static string ClientId;

		private static string Version;

		public AnalyticsHelper(Guid guid, string clientId,string version) : base(Account, new SimpleTrackerEnvironment(Environment.OSVersion.Platform.ToString(), Environment.OSVersion.Version.ToString(), Environment.OSVersion.VersionString))
		{
			Guid = guid;
			ClientId = clientId;
			Version = version;

			UserAgent = "Mozilla/5.0 (compatible;.NET CLR 4.0.30319; osi)";
#if DEBUG
			EndpointUrl = GoogleAnalyticsEndpoints.Debug;
#endif
		}

		protected override void AmendParameters(IGeneralParameters parameters)
		{
			parameters.ApplicationName = "osi";
			parameters.ApplicationId = "osi.Desktop";
			parameters.ApplicationVersion = Version;
			parameters.IpOverride = Ip;

			//parameters.ClientId = ClientId;

			parameters.UserId = Guid.ToString();
		}
	}
}
