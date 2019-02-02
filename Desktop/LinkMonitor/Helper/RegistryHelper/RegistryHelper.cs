using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace osi.Desktop.Helper
{
	public delegate void ShowHelperDialog(OSVersion version);
	public enum OSVersion
	{
		//https://docs.microsoft.com/zh-cn/windows/desktop/SysInfo/operating-system-version
		Windows_10_Above_And_Include_Build10122,
		Windows_10_Below_Build10122,
		Windows_8_And_Above,
		Windows_7_And_Vista,
		Windows_2000_And_Xp,
		Unknow,
		Unset
	}
	public class RegistryHelper
    {
		#region Private Members


		private BrowserRegistry mUserBrowserRegistry = new BrowserRegistry();

		#endregion

		#region Public Members

		public static OSVersion osVersion = OSVersion.Unset;

		public ShowHelperDialog ShowHelperDialog = null;
		public BrowserRegistry UserBrowserRegistry { get { return mUserBrowserRegistry; } }
		public osiBrowserRegistry osiBrowserRegistry { get; set; } = new osiBrowserRegistry();

		#endregion

		public RegistryHelper()
        {
            if (osVersion == OSVersion.Unset)
            {
                osVersion = GetOSVersion(Environment.OSVersion.Version);
            }

			// 初始化 osiBrowserRegistry
			string path = Environment.CurrentDirectory;

			osiBrowserRegistry.http_ApplicationIconPath = osiBrowserRegistry.https_ApplicationIconPath = '"' + path + @"\osi.exe" + '"' + ",0";
			osiBrowserRegistry.http_Command = osiBrowserRegistry.https_Command = '"' + path + @"\LinkMonitor.exe" + '"' + "--Link" + " " + '"' + "%1" + '"';
		}

		/// <summary>
		/// 注册为默认浏览器
		/// </summary>
		/// <param name="registry">如果为 null 则为 FirstRun</param>
		public void Register(BrowserRegistry registry = null)
        {
			/// 两个方案
			/// 1.Windows_10_Above_And_Include_Build10122 将osi注册为默认浏览器并 *不修改* 回原浏览器
			/// 2.Windows_10_Below_Build10122 Windows_8_And_Above Windows_7_And_Vista 将osi注册为默认浏览器并 *修改* 回原浏览器

			bool IsFirstRun = registry == null ? true : false;

			//Is First Run And Check OSVersion
            if (IsFirstRun && ShouldRegisterAsBrowser()) RegisterAsBrowser();

			GetUserBrowserRegistry();
            RegisterAsDefaultBrowser(registry);

			if (IsFirstRun) GetHash();
		}

		/// <summary>
		/// 恢复用户默认浏览器设置
		/// </summary>
		/// <param name="registry"></param>
		public void UnRegister(BrowserRegistry registry)
		{
			RegisterAsDefaultBrowser(UserBrowserRegistry);
		}

		#region RegisterAsBrowser
		/// <summary>
		/// 在注册表内注册为一个浏览器
		/// </summary>
		private bool ShouldRegisterAsBrowser()
		{
			return (osVersion == OSVersion.Windows_10_Above_And_Include_Build10122 || osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista);
		}
		private void RegisterAsBrowser()
		{
			string path = Environment.CurrentDirectory;

			string osiIconPath = '"' + path + @"\osi.exe" + '"' + ",0";

			string lmPath = '"' + path + @"\LinkMonitor.exe" + '"';
			string lmPathWithArg = lmPath + "--Link" + " " + '"' + "%1" + '"';

			RegistryKey osiURL_CU = Registry.CurrentUser.OpenSubKey(@"Software\Classes", true).CreateSubKey("osiURL");
			osiURL_CU.SetValue("", "osi Url Handle");
			osiURL_CU.CreateSubKey("DefaultIcon").SetValue("", osiIconPath);
			osiURL_CU.CreateSubKey(@"shell\open\command").SetValue("", lmPathWithArg);
			RegistryKey Application = osiURL_CU.CreateSubKey("Application");
			Application.SetValue("ApplicationIcon", osiIconPath);
			Application.SetValue("ApplicationName", "osi Link Monitor");
			Application.Close();
			osiURL_CU.Close();

			RegistryKey osi_CU = Registry.CurrentUser.OpenSubKey(@"Software\Clients\StartMenuInternet", true).CreateSubKey("osi");
			osi_CU.CreateSubKey(@"shell\open\command").SetValue("", lmPathWithArg);
			osi_CU.CreateSubKey(@"Capabilities\URLAssociations");
			RegistryKey Capabilities_CU = osi_CU.OpenSubKey("Capabilities", true);
			Capabilities_CU.SetValue("AppUserModelId", "osi");
			Capabilities_CU.SetValue("ApplicationName", "osi");
			Capabilities_CU.SetValue("ApplicationIcon", osiIconPath);
			Capabilities_CU.SetValue("ApplicationDescription", "A osu! Beatmap Downloader");
			RegistryKey URLAssociations_CU = Capabilities_CU.OpenSubKey("URLAssociations", true);
			URLAssociations_CU.SetValue("http", "osiURL");
			URLAssociations_CU.SetValue("https", "osiURL");
			osi_CU.Close();
			Capabilities_CU.Close();
			URLAssociations_CU.Close();

			Registry.SetValue(@"HKEY_CURRENT_USER\Software\RegisteredApplications", "osi", @"Software\Clients\StartMenuInternet\osi\Capabilities");
		}
		#endregion

		#region RegisterAsDefaultBrowser
		/// <summary>
		/// 注册为默认浏览器
		/// </summary>
		private void RegisterAsDefaultBrowser(BrowserRegistry registry)
		{
			switch (osVersion)
			{
				case OSVersion.Windows_10_Above_And_Include_Build10122:
					rRegisterAsDefaultBrowser_Windows_10_Above_And_Include_Build10122(registry);
					break;
				case OSVersion.Windows_10_Below_Build10122:
				case OSVersion.Windows_8_And_Above:
				case OSVersion.Windows_7_And_Vista:
					rRegisterAsDefaultBrowser_Windows_10_Below_Build10122_Windows_7_And_Vista_Above(registry);
					break;
				case OSVersion.Windows_2000_And_Xp:
					rRegisterAsDefaultBrowser_Windows_2000_And_Xp(registry);
					break;
				default:
					throw new Exception("Unsupported");
			}
		}
		private void rRegisterAsDefaultBrowser_Windows_10_Above_And_Include_Build10122(BrowserRegistry registry)
		{
			if (registry == null)
			{
				RegistryKey httpUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", true);
				RegistryKey httpsUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", true);

				httpUserChoice.SetValue("Progid", "NotosiURL");
				httpsUserChoice.SetValue("Progid", "NotosiURL");
			};

		}
		private void rRegisterAsDefaultBrowser_Windows_10_Below_Build10122_Windows_7_And_Vista_Above(BrowserRegistry registry)
		{
			if (registry == null) registry = osiBrowserRegistry;

			RegistryKey httpUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", true);
			RegistryKey httpsUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", true);

			httpUserChoice.SetValue("Progid", registry.http_Progid);
			httpsUserChoice.SetValue("Progid", registry.https_Progid);

			if (registry.http_Hash != null && registry.https_Hash != null)
			{
				httpUserChoice.SetValue("Hash", registry.http_Hash);
				httpsUserChoice.SetValue("Hash", registry.https_Hash);
			}
		}
		private void rRegisterAsDefaultBrowser_Windows_2000_And_Xp(BrowserRegistry registry)
		{
			if (registry == null) registry = osiBrowserRegistry;

			RegistryKey http = Registry.CurrentUser.OpenSubKey(@"Software\Classes\http", true);
			http.OpenSubKey("DefaultIcon", true).SetValue("", registry.http_ApplicationIconPath);
			http.OpenSubKey(@"shell\open\command", true).SetValue("", registry.http_Command);
			http.Close();
			RegistryKey https = Registry.CurrentUser.OpenSubKey(@"Software\Classes\https", true);
			https.OpenSubKey("DefaultIcon", true).SetValue("", registry.https_ApplicationIconPath);
			https.OpenSubKey(@"shell\open\command", true).SetValue("", registry.https_Command);
			https.Close();
		}

		#endregion

		/// <summary>
		/// 获取当前默认浏览器的信息
		/// </summary>
		private void GetUserBrowserRegistry()
		{
			if (osVersion == OSVersion.Windows_10_Above_And_Include_Build10122 || osVersion == OSVersion.Windows_10_Above_And_Include_Build10122 || osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista)
			{
				RegistryKey httpUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", true);
				RegistryKey httpsUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", true);

				mUserBrowserRegistry.http_Progid = httpUserChoice.GetValue("Progid").ToString();
				mUserBrowserRegistry.https_Progid = httpsUserChoice.GetValue("Progid").ToString();

				mUserBrowserRegistry.http_Hash = httpUserChoice.GetValue("Hash").ToString();
				mUserBrowserRegistry.https_Hash = httpsUserChoice.GetValue("Hash").ToString();

				RegistryKey http = Registry.CurrentUser.OpenSubKey(@"Software\Classes\" + mUserBrowserRegistry.http_Progid + @"\Shell\open\command");
				RegistryKey https = Registry.CurrentUser.OpenSubKey(@"Software\Classes\" + mUserBrowserRegistry.https_Progid + @"\Shell\open\command");
				mUserBrowserRegistry.http_Command = http.GetValue("").ToString();
				mUserBrowserRegistry.https_Command = http.GetValue("").ToString();

				RegistryKey httpUserBrowser = Registry.CurrentUser.OpenSubKey(Registry.GetValue(@"HKEY_CURRENT_USER\Software\RegisteredApplications", mUserBrowserRegistry.http_Progid, null)?.ToString() + @"\Capabilities");
				RegistryKey httpsUserBrowser = Registry.CurrentUser.OpenSubKey(Registry.GetValue(@"HKEY_CURRENT_USER\Software\RegisteredApplications", mUserBrowserRegistry.https_Progid, null)?.ToString() + @"\Capabilities");

				mUserBrowserRegistry.http_ApplicationName = httpUserBrowser?.GetValue("ApplicationName")?.ToString();
				mUserBrowserRegistry.https_ApplicationName = httpsUserBrowser?.GetValue("ApplicationName")?.ToString();

				mUserBrowserRegistry.http_ApplicationIconPath = httpUserBrowser?.GetValue("ApplicationIcon")?.ToString();
				mUserBrowserRegistry.https_ApplicationIconPath = httpsUserBrowser?.GetValue("ApplicationIcon")?.ToString();

				mUserBrowserRegistry.http_ApplicationDescriptione = httpUserBrowser?.GetValue("ApplicationDescription")?.ToString();
				mUserBrowserRegistry.https_ApplicationDescriptione = httpsUserBrowser?.GetValue("ApplicationDescription")?.ToString();
			}
			else if (osVersion == OSVersion.Windows_2000_And_Xp)
			{
				RegistryKey http = Registry.CurrentUser.OpenSubKey(@"Software\Classes\http", true);
				mUserBrowserRegistry.http_ApplicationIconPath = (string)http.OpenSubKey("DefaultIcon", true).GetValue("");
				mUserBrowserRegistry.http_Command = (string)http.OpenSubKey(@"shell\open\command", true).GetValue("");
				http.Close();

				RegistryKey https = Registry.CurrentUser.OpenSubKey(@"Software\Classes\https", true);
				mUserBrowserRegistry.https_ApplicationIconPath = (string)https.OpenSubKey("DefaultIcon", true).GetValue("");
				mUserBrowserRegistry.https_Command = (string)https.OpenSubKey(@"shell\open\command", true).GetValue("");
				https.Close();
			}
		}

		#region Get Hasah
		private void GetHash()
		{
			ShowHelperDialog?.Invoke(osVersion);

			switch (osVersion)
			{
				case OSVersion.Windows_10_Above_And_Include_Build10122:
					gGetHash_Windows_10_Above_And_Include_Build10122();
					break;
				case OSVersion.Windows_10_Below_Build10122:
				case OSVersion.Windows_8_And_Above:
					gGetHash_Windows_10_Below_Build10122_Windows_8_And_Above();
					break;
				case OSVersion.Windows_7_And_Vista:
				case OSVersion.Windows_2000_And_Xp:
					osiBrowserRegistry.http_Hash = null;
					osiBrowserRegistry.https_Hash = null;
					break;
				default:
					throw new Exception("Unsupported");
			}
		}

		private void gGetHash_Windows_10_Above_And_Include_Build10122()
		{
			Process.Start("ms-settings:defaultapps");
			bool getHttpHash = false, getHttpsHash = false;
			while (!(getHttpHash && getHttpsHash))
			{
				getHttpHash = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "Progid", null)?.ToString() == "osiURL";
				getHttpsHash = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", "Progid", null)?.ToString() == "osiURL";
				Thread.Sleep(1000);
			}

		}
		private void gGetHash_Windows_10_Below_Build10122_Windows_8_And_Above()
		{
			NamedPipeServerStream server = new NamedPipeServerStream("osi", PipeDirection.In);

			Process.Start(@"http://config.osi/getHttpHash");
			bool getHttpHash = false, getHttpsHash = false;
			while (!(getHttpHash && getHttpsHash))
			{
				server.WaitForConnection();

				StreamReader sr = new StreamReader(server);
				string link = sr.ReadToEnd();
				if (link == "http://config.osi/getHttpHash")
				{
					getHttpHash = true;
					Process.Start(@"https://config.osi/getHttpsHash");
				}
				else if (link == "https://config.osi/getHttpsHash")
				{
					getHttpsHash = true;
				}
				server.Disconnect();
			}

			server.Dispose();
			RegistryKey httpUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", true);
			RegistryKey httpsUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", true);
			osiBrowserRegistry.http_Hash = httpUserChoice.GetValue("Hash").ToString();
			osiBrowserRegistry.https_Hash = httpsUserChoice.GetValue("Hash").ToString();

		}
		private void gGetHash_Windows_7_And_Vista()
		{
			Process.Start("Control", "/name Microsoft.DefaultPrograms /page pageFileAssoc");
			bool getHttpHash = false, getHttpsHash = false;
			while (!(getHttpHash && getHttpsHash))
			{
				getHttpHash = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "Progid", null)?.ToString() == "osiURL";
				getHttpsHash = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", "Progid", null)?.ToString() == "osiURL";
				Thread.Sleep(1000);
			}

		}

		#endregion

		public static OSVersion GetOSVersion(Version osVersion)
        {
            if (osVersion.Major == 10 && osVersion.Build >= 10122)
            {
                return OSVersion.Windows_10_Above_And_Include_Build10122;
            }
            else if (osVersion.Major == 10 && osVersion.Build < 10122)
            {
                return OSVersion.Windows_10_Below_Build10122;
            }
            else if (osVersion.Major == 6 && osVersion.Minor >= 2)
            {
                return OSVersion.Windows_8_And_Above;
            }
            else if ((osVersion.Major == 6 && osVersion.Minor < 2))
            {
                return OSVersion.Windows_7_And_Vista;
            }
            else if (osVersion.Minor >= 5)
            {
                return OSVersion.Windows_2000_And_Xp;
            }
            else
            {
                return OSVersion.Unknow;
            }
        }

    }
}
