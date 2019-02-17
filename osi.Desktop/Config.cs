using osi.Desktop.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace osi.Desktop
{
	public class Config:osi.Core.BaseViewModel
	{
		public static Config Current { get; set; }

		public static bool IsInstall;

		#region Property

		public RegistryKeyPair Registry = new RegistryKeyPair();

		public OSVersion OSVersion { get; set; }

		public bool IsRunning { get; set; }

		public string Version { get; set; }

		public Guid Guid { get; set; }

		public string ClientId { get; set; }

		#endregion

		public static Config Load(string path = "osi.Desktop.config.xml")
		{
			XmlSerializer s = new XmlSerializer(typeof(Config));
			try
			{
				using(var f = File.OpenText(path))
				{
					return (Config)s.Deserialize(f);
				}
			}
			catch
			{
				IsInstall = true;
				return new Config();
			}
		}
		public void Save(string path = "osi.Desktop.config.xml")
		{
			XmlSerializer s = new XmlSerializer(typeof(Config));
			try
			{
				using (var f = File.CreateText(path))
				{
					s.Serialize(f, this);
				}
			}
			catch { }
		}

	}
	public class RegistryKeyPair
	{
		public string osuPath { get; set; }

		public osiBrowserRegistry osiBrowserRegistry { get; set; }

		public BrowserRegistry UserBrowserRegistry { get; set; }
	}

}
