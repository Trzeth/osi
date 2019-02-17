using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace osi.Desktop.Helper
{
	public class UpdateHelper
	{
		private UpdateModel UpdateModel;
		private WebClient WebClient = new WebClient();

		public const string BaseUri = "https://osi.nyao.kim";

		public UpdateHelper()
		{
			UpdateModel = JsonConvert.DeserializeObject<UpdateModel>(WebClient.DownloadString($"{BaseUri}/api/"));
		}

		public bool HasUpdate(string version)
		{
			return (version != UpdateModel.Version.ToString());
		}

		public void DownloadUpdateFile()
		{
			foreach(KeyValuePair<string,string> pair in UpdateModel.Files)
			{
				WebClient.DownloadFile(pair.Value,$"{Environment.CurrentDirectory}\\{pair.Key}.new");
			}
		}
	}
}
