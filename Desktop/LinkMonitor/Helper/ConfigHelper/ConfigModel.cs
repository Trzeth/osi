using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LinkMonitor.Helper
{
	[XmlRootAttribute("Config", Namespace = "https://osi.nyao.kim",IsNullable = true)]
	public class ConfigModel
	{
		public RegistryKeyPair Registry = new RegistryKeyPair();

		public OSVersion OSVersion { get; set; }

		public bool IsRunning { get; set; }
	}
	public class RegistryKeyPair
	{
		public osiBrowserRegistry osiBrowserRegistry { get; set; }

		public BrowserRegistry UserBrowserRegistry { get; set; }
	}

}
