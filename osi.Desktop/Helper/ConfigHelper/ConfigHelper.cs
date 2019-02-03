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
		private ConfigModel mCondigModel;

		public ConfigModel ConfigModel
		{
			get { return mCondigModel; }
			set { mCondigModel = value; }
		}
		public const string ConfigFileName = "osi.Desktop.config.xml";
		public string ConfigFilePath = Environment.CurrentDirectory + @"\" + ConfigFileName;

		public ConfigHelper() { }
		public bool ReadConfigFromFile()
		{
			if (!File.Exists(ConfigFilePath)) return false;

			try
			{
				TextReader reader = new StreamReader(ConfigFilePath);
				XmlSerializer s = new XmlSerializer(typeof(ConfigModel));
				var configModel = (ConfigModel)s.Deserialize(reader);
				reader.Close();
				mCondigModel = configModel;
			}
			catch(InvalidOperationException)
			{
				return false;
			}
			return true;
		}
		public void SaveConfig(ConfigModel configModel =  null)
		{
			if (configModel == null) configModel = this.ConfigModel;
			TextWriter writer = new StreamWriter(ConfigFilePath);
			XmlSerializer s = new XmlSerializer(typeof(ConfigModel));
			s.Serialize(writer, configModel);
			writer.Close();
		}
		/// <summary>
		/// 如果改变后为 running 返回 true
		/// 否则为 false 
		/// </summary>
		/// <returns></returns>
		public void ChangeRunningStatus(bool IsRunning)
		{
			if (mCondigModel == null) ReadConfigFromFile();
			ConfigModel configModel = mCondigModel;

			configModel.IsRunning = IsRunning;
		}
	}
}
