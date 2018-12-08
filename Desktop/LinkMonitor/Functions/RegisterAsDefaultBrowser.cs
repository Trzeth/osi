using Microsoft.Win32;
using System;
using static LinkMonitor.Program;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinkMonitor.Functions
{
    static class RegisterAsDefaultBrowserFunction
    {
        static OSVersion osVersion = OSVersion.Unset;
        static RegisterAsDefaultBrowserFunction()
        {
            if (osVersion == OSVersion.Unset)
            {
                osVersion = GetOSVersion(Environment.OSVersion.Version);
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
            //SHChangeNotify();
        }
        internal static void Register()
        {
            if (osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista)
            {
                RegisterAsBrowser();
            }

            RegisterAsDefaultBrowser();

            if (osVersion == OSVersion.Windows_10_Above_And_Include_Build10122) //Windows 10
            {
                exitCode = ExitCode.HandledError;
            }
            else if (osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista)  //Windows 8 Windows 10.0.10122
            {
                if (osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above)
                {
                    //Windows 10 与 Windows 8 需要记录 Hash
                    exitCode = ExitCode.Continue;
                }
                else
                {
                    //Windows 7 及 Vista 不需要
                    exitCode = ExitCode.Succeed;
                }

            }
            else if (osVersion == OSVersion.Windows_2000_And_Xp)
            {
                exitCode = ExitCode.Alert;
            }
            else
            {
                //不支持的系统
                exitCode = ExitCode.HandledError;
            }
        }
        internal static void RegisterAsDefaultBrowser()
        {
            RegisterAsDefaultBrowser(null, null);
        }
        internal static void RegisterAsDefaultBrowser(string httpHash,string httpsHash)
        {
            if (osVersion == OSVersion.Windows_10_Above_And_Include_Build10122)
            {
                exitCode = ExitCode.HandledError;
            }
            else if (osVersion == OSVersion.Windows_10_Below_Build10122 || osVersion == OSVersion.Windows_8_And_Above || osVersion == OSVersion.Windows_7_And_Vista)
            {
                RegistryKey httpUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", true);
                RegistryKey httpsUserChoice = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", true);

                httpUserChoice.SetValue("Progid", "osiURL");
                httpsUserChoice.SetValue("Progid", "osiURL");

                if (httpHash != null && httpsHash != null)
                {
                    httpUserChoice.SetValue("Hash", httpHash);
                    httpsUserChoice.SetValue("Hash", httpsHash);
                }
            }
            else if (osVersion == OSVersion.Windows_2000_And_Xp)
            {
                string path = Environment.CurrentDirectory;

                string osiIconPath = '"' + path + @"\osi.exe" + '"' + ",0";
                string lmPath = '"' + path + @"\LinkMonitor.exe" + '"';
                string lmPathWithArg = lmPath + "--Link" + " " + '"' + "%1" + '"';

                RegistryKey http = Registry.CurrentUser.OpenSubKey(@"Software\Classes\http", true);
                http.OpenSubKey("DefaultIcon", true).SetValue("", osiIconPath);
                http.OpenSubKey(@"shell\open\command", true).SetValue("", lmPathWithArg);
                http.Close();
                RegistryKey https = Registry.CurrentUser.OpenSubKey(@"Software\Classes\http", true);
                https.OpenSubKey("DefaultIcon", true).SetValue("", osiIconPath);
                https.OpenSubKey(@"shell\open\command", true).SetValue("", lmPathWithArg);
                https.Close();
            }
            else
            {
                exitCode = ExitCode.HandledError;
            }

            //SHChangeNotify();
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
