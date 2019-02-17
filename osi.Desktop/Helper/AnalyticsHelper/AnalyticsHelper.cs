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
using GoogleAnalyticsTracker.Core.Interface;

namespace osi.Desktop.Helper
{
	public class Session : IAnalyticsSession
	{
		private Guid Guid = Guid.NewGuid();
		public string GenerateCacheBuster()
		{
			throw new NotImplementedException();
		}

		public string GenerateSessionId()
		{
			return Guid.ToString();
		}
	}
	public class AnalyticsHelper : SimpleTracker
	{
		public static AnalyticsHelper Current { get; set; }

		private const string Account = "UA-121062181-1";
		private static Session Session = new Session();

		//Guid = UserId per app
		private static Guid Guid;
		//ClientId = UserName per player
		private static string ClientId;

		private static string Version;

		public AnalyticsHelper(Guid guid, string clientId,string version) : base(Account,Session,new SimpleTrackerEnvironment(Environment.OSVersion.Platform.ToString(), Environment.OSVersion.Version.ToString(), Environment.OSVersion.VersionString))
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

			parameters.UserId = Guid.ToString();
		}
	}
}
