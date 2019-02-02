using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace osi.Desktop.Helper
{
	public class ConfigHelper
	{
		private ConfigModel mCondigModel = new ConfigModel();

		public ConfigModel ConfigModel { get { return mCondigModel; } }
		public const string ConfigFileName = "osi.Desktop.config.xml";

		public ConfigHelper() { }
		public bool IsConfigFileExist()
		{
			return File.Exists(ConfigFileName);
		}
		public ConfigModel GetConfigFromFile()
		{
			TextReader reader = new StreamReader(ConfigFileName);
			XmlSerializer s = new XmlSerializer(typeof(ConfigModel));
			var configModel = (ConfigModel)s.Deserialize(reader);
			reader.Close();
			return configModel;
		}
		public void SaveConfig(ConfigModel configModel)
		{
			TextWriter writer = new StreamWriter(ConfigFileName);
			XmlSerializer s = new XmlSerializer(typeof(ConfigModel));
			s.Serialize(writer, configModel);
			writer.Close();
		}
		/// <summary>
		/// 如果改变后为 running 返回 true
		/// 否则为 false 
		/// </summary>
		/// <returns></returns>
		public bool ChangeRunningStatus()
		{
			bool IsRunning = new bool();
			ConfigModel configModel = GetConfigFromFile();

			IsRunning = configModel.IsRunning = !configModel.IsRunning;
			SaveConfig(configModel);

			return IsRunning;
		}
	}
}
