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

namespace osi_Console.Functions
{
    static class RegisterAsDefaultBrowserFunction
    {
        static string Windows_2000_And_Xp_http_IconPath;
        static string Windows_2000_And_Xp_https_IconPath;
        static string Windows_2000_And_Xp_http_command;
        static string Windows_2000_And_Xp_https_command;

        //Unfinish
        static string Windows_7_And_Above_http_Progid;
        static string Windows_7_And_Above_http_Hash;
        static string Windows_7_And_Above_https_Progid;
        static string Windows_7_And_Above_https_Hash;

        static OSVersion osVersion = OSVersion.Unset;
        static RegisterAsDefaultBrowserFunction()
        {
            if (osVersion == OSVersion.Unset)
            {
                osVersion = GetOSVersion(Environment.OSVersion.Version);
            }
        }
        internal static void Register(out string HttpHash,out string HttpsHash)
        {
            if (osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista)
            {
                RegisterAsBrowser();
            }

            RegisterAsDefaultBrowser(null,null);

            if (osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista)
            {
                GetHash(out HttpHash,out HttpsHash);
            }else
            {
                HttpHash = null;
                HttpsHash = null;
            }
        }
        private static void RegisterAsBrowser()
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

            RegistryKey osi_CU = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("osi");
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

            Registry.SetValue(@"HKEY_CURRENT_USER\Software\RegisteredApplications", "osi", @"Software\osi\Capabilities");
        }
        internal static void RegisterAsDefaultBrowser(string HttpHash,string HttpsHash)
        {
            if (osVersion == OSVersion.Windows_10_Above_And_Include_Build10122)
            {
                throw new Exception("UnSupport");
            }
            else if (osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista)
            {
                RegistryKey httpUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", true);
                RegistryKey httpsUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", true);

                httpUserChoice.SetValue("Progid", "osiURL");
                httpsUserChoice.SetValue("Progid", "osiURL");

                if (HttpHash != null && HttpsHash != null)
                {
                    httpUserChoice.SetValue("Hash", HttpHash);
                    httpsUserChoice.SetValue("Hash", HttpsHash);
                }
            }
            else if (osVersion == OSVersion.Windows_2000_And_Xp)
            {
                string path = Environment.CurrentDirectory;

                string osiIconPath = '"' + path + @"\osi.exe" + '"' + ",0";
                string lmPath = '"' + path + @"\LinkMonitor.exe" + '"';
                string lmPathWithArg = lmPath + "--Link" + " " + '"' + "%1" + '"';

                RegistryKey http = Registry.CurrentUser.OpenSubKey(@"Software\Classes\http", true);
                Windows_2000_And_Xp_http_IconPath = (string)http.OpenSubKey("DefaultIcon", true).GetValue("");
                http.OpenSubKey("DefaultIcon", true).SetValue("", osiIconPath);
                Windows_2000_And_Xp_http_command = (string)http.OpenSubKey(@"shell\open\command", true).GetValue("");
                http.OpenSubKey(@"shell\open\command", true).SetValue("", lmPathWithArg);
                http.Close();
                RegistryKey https = Registry.CurrentUser.OpenSubKey(@"Software\Classes\https", true);
                Windows_2000_And_Xp_https_IconPath = (string)https.OpenSubKey("DefaultIcon", true).GetValue("");
                https.OpenSubKey("DefaultIcon", true).SetValue("", osiIconPath);
                Windows_2000_And_Xp_https_command = (string)https.OpenSubKey(@"shell\open\command", true).GetValue("");
                https.OpenSubKey(@"shell\open\command", true).SetValue("", lmPathWithArg);
                https.Close();
            }
            else
            {
                //exitCode = ExitCode.HandledError;
            }
        }
        internal static void GetHash(out string HttpHash,out string HttpsHash)
        {
            NamedPipeServerStream server = new NamedPipeServerStream("osi", PipeDirection.In);

            Console.WriteLine("请在之后出现的选项中选择 osi Link Monitor。");
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
            HttpHash = httpUserChoice.GetValue("Hash").ToString();
            HttpsHash = httpsUserChoice.GetValue("Hash").ToString();
        }
        private enum OSVersion
        {
            // https://docs.microsoft.com/zh-cn/windows/desktop/SysInfo/operating-system-version
            Windows_10_Above_And_Include_Build10122,
            Windows_10_Below_Build10122,
            Windows_8_And_Above,
            Windows_7_And_Vista,
            Windows_2000_And_Xp,
            Unknow,
            Unset
        }
        private static OSVersion GetOSVersion(Version osVersion)
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
