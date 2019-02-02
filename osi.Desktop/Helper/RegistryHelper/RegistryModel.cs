using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace osi.Desktop.Helper
{
	public class osiBrowserRegistry : BrowserRegistry
	{
		public osiBrowserRegistry(string http_Hash, string https_Hash) : base()
		{
			http_Progid = "osiURL";
			https_Progid = "osiURL";

			base.http_Hash = http_Hash;
			base.https_Hash = https_Hash;
		}
		public osiBrowserRegistry() : base()
		{
			http_Progid = "osiURL";
			https_Progid = "osiURL";
		}
	}

	[XmlInclude(typeof(osiBrowserRegistry))]
	public class BrowserRegistry
	{
		/// <summary>
		/// OSVerion > OSVersion.Windows_7_And_Vista 才有 Hash 否则为 null
		/// </summary>
		public string http_Progid;
		public string http_Hash;
		public string https_Progid;
		public string https_Hash;

		public string http_ApplicationName;
		public string https_ApplicationName;

		public string http_ApplicationIconPath;
		public string https_ApplicationIconPath;

		public string http_ApplicationDescriptione;
		public string https_ApplicationDescriptione;

		public string http_Command;
		public string https_Command;

		public BrowserRegistry() { }

		public void OpenUrl(Uri uri)
		{

		}
	}
}
