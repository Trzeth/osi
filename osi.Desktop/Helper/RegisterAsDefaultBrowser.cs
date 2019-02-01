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

namespace osi.Desktop
{
	public class osiBrowserRegistry : BrowserRegistry
	{
		public osiBrowserRegistry() : base()
		{
			http_Progid = "osiURL";
			https_Progid = "osiURL";
		}
	}
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
	public class RegisterHelper
    {
		#region Private Members

		private static OSVersion osVersion = OSVersion.Unset;

		private BrowserRegistry mUserBrowserRegistry = new BrowserRegistry();

		#endregion

		#region Public Members

		public EventHandler<EventArgs> OpeningSettingPage;
		public void OnOpeningSettingPage(EventArgs e)
		{
			OpeningSettingPage?.Invoke(this, e);
		}
		public BrowserRegistry UserBrowserRegistry { get { return mUserBrowserRegistry; } }
		public BrowserRegistry osiBrowserRegistry { get; set; } = new osiBrowserRegistry();

		#endregion

		public RegisterHelper()
        {
            if (osVersion == OSVersion.Unset)
            {
                osVersion = GetOSVersion(Environment.OSVersion.Version);
            }

			// 初始化 osiBrowserRegistry
			string path = Environment.CurrentDirectory;

			osiBrowserRegistry.https_ApplicationIconPath = '"' + path + @"\osi.exe" + '"' + ",0";
			osiBrowserRegistry.http_Command = osiBrowserRegistry.https_Command = '"' + path + @"\LinkMonitor.exe" + '"' + "--Link" + " " + '"' + "%1" + '"';
		}
		/// <summary>
		/// 注册为默认浏览器
		/// </summary>
		/// <param name="registry"></param>
		public void Register(BrowserRegistry registry)
        {
			/// 两个方案
			/// 1.Windows_10_Above_And_Include_Build10122 将osi注册为默认浏览器并 *不修改* 回原浏览器
			/// 2.Windows_10_Below_Build10122 Windows_8_And_Above Windows_7_And_Vista 将osi注册为默认浏览器并 *修改* 回原浏览器
			 
			bool IsFirstRun = false;

			if (registry == null)
			{
				IsFirstRun = true;
				registry = osiBrowserRegistry;
			}


			//Is First Run And Check OSVersion
            if (IsFirstRun && (osVersion == OSVersion.Windows_10_Above_And_Include_Build10122 || osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista))
            {
                RegisterAsBrowser();
            }

			GetUserBrowserRegistry();
            RegisterAsDefaultBrowser(registry);

			if (IsFirstRun)
			{
				if (osVersion == OSVersion.Windows_10_Above_And_Include_Build10122)
				{
					GetHash();

					osiBrowserRegistry.http_Hash = null;
					osiBrowserRegistry.https_Hash = null;
				}
				else if (osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista)
				{
					GetHash();
				}
				else
				{
					osiBrowserRegistry.http_Hash = null;
					osiBrowserRegistry.https_Hash = null;
				}
			}
		}

		/// <summary>
		/// 恢复用户默认浏览器设置
		/// </summary>
		/// <param name="registry"></param>
		public void UnRegister(BrowserRegistry registry)
		{
			Register(UserBrowserRegistry);
		}

		/// <summary>
		/// 在注册表内注册为一个浏览器
		/// </summary>
        private void RegisterAsBrowser()
        {
            string path = Environment.CurrentDirectory;

            string osiIconPath = '"' + path + @"\osi.exe" + '"' + ",0";

			string lmPath = '"' + path + @"\LinkMonitor.exe" + '"';
            string lmPathWithArg = lmPath + "--Link" + " " + '"' + "%1" + '"';

            RegistryKey osiURL_CU = Registry.CurrentUser.OpenSubKey(@"Software\Classes",true).CreateSubKey("osiURL");
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

		/// <summary>
		/// 注册为默认浏览器
		/// </summary>
		/// <param name="HttpHash"></param>
		/// <param name="HttpsHash"></param>
        private void RegisterAsDefaultBrowser(BrowserRegistry registry)
        {
			if (osVersion == OSVersion.Windows_10_Above_And_Include_Build10122)
			{
				//No thing to do
			}
			if (osVersion == OSVersion.Windows_10_Above_And_Include_Build10122||osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista)
            {
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
            else if (osVersion == OSVersion.Windows_2000_And_Xp)
            {
                RegistryKey http = Registry.CurrentUser.OpenSubKey(@"Software\Classes\http", true);
                http.OpenSubKey("DefaultIcon", true).SetValue("", registry.http_ApplicationIconPath);
                http.OpenSubKey(@"shell\open\command", true).SetValue("", registry.http_Command);
                http.Close();
                RegistryKey https = Registry.CurrentUser.OpenSubKey(@"Software\Classes\https", true);
                https.OpenSubKey("DefaultIcon", true).SetValue("", registry.https_ApplicationIconPath);
                https.OpenSubKey(@"shell\open\command", true).SetValue("", registry.https_Command);
                https.Close();
            }
            else
            {
				throw new Exception("Unsupported");
            }
        }

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

				RegistryKey http = Registry.CurrentUser.OpenSubKey(@"Software\Classes\" + mUserBrowserRegistry.http_Progid +@"\Shell\open\command");
				RegistryKey https = Registry.CurrentUser.OpenSubKey(@"Software\Classes\" + mUserBrowserRegistry.https_Progid + @"\Shell\open\command");
				mUserBrowserRegistry.http_Command = http.GetValue("").ToString();
				mUserBrowserRegistry.https_Command = http.GetValue("").ToString();

				RegistryKey httpUserBrowser = Registry.CurrentUser.OpenSubKey(Registry.GetValue(@"HKEY_CURRENT_USER\Software\RegisteredApplications", mUserBrowserRegistry.http_Progid, null).ToString() + @"\Capabilities");
				RegistryKey httpsUserBrowser = Registry.CurrentUser.OpenSubKey(Registry.GetValue(@"HKEY_CURRENT_USER\Software\RegisteredApplications", mUserBrowserRegistry.https_Progid, null).ToString() + @"\Capabilities");

				mUserBrowserRegistry.http_ApplicationName = httpUserBrowser.GetValue("ApplicationName").ToString();
				mUserBrowserRegistry.https_ApplicationName = httpsUserBrowser.GetValue("ApplicationName").ToString();

				mUserBrowserRegistry.http_ApplicationIconPath = httpUserBrowser.GetValue("ApplicationIcon").ToString();
				mUserBrowserRegistry.https_ApplicationIconPath = httpsUserBrowser.GetValue("ApplicationIcon").ToString();

				mUserBrowserRegistry.http_ApplicationDescriptione = httpUserBrowser.GetValue("ApplicationDescription").ToString();
				mUserBrowserRegistry.https_ApplicationDescriptione = httpsUserBrowser.GetValue("ApplicationDescription").ToString();
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

		private void GetHash()
        {
            NamedPipeServerStream server = new NamedPipeServerStream("osi", PipeDirection.In);

			OnOpeningSettingPage(new EventArgs());

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
            RegistryKey httpUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", true);
            RegistryKey httpsUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", true);
            osiBrowserRegistry.http_Hash = httpUserChoice.GetValue("Hash").ToString();
			osiBrowserRegistry.https_Hash = httpsUserChoice.GetValue("Hash").ToString();
        }

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
