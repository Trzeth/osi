using Colorful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace osi_Console.Functions
{
    static class InputCookieFunction
    {
        private enum Status
        {
            Ok,
            Fail,
            Skip
        }
        internal static bool InputInsoCookie(out string Cookie)
        {
            Console.WriteLine("请输入浏览器储存在 inso.link 域名下命名为 do_not_remove_this_0w0 的Cookie 值。");
            Console.WriteLine("或按 回车键 跳过。");
            while (true)
            {
                string s = Console.ReadLine();
                if (s != "")
                {
                    if (DownloadEngine.Servers.Inso.IsCookieValid(s))
                    {
                        WriteStatus(Status.Ok);
                        Cookie = s;
                        return true;
                    }
                    else
                    {
                        WriteStatus(Status.Fail);
                    }
                }
                else
                {
                    WriteStatus(Status.Skip);
                    Cookie = null;
                    return false;
                }
            }
        }
        internal static bool InputBloodcatCookie(out string Cookie)
        {
            Console.WriteLine("请输入浏览器储存在 bloodcat.com 域名下命名为 obm_human 的Cookie 值。");
            Console.WriteLine("或按 回车键 跳过。");
            while (true)
            {
                string s = Console.ReadLine();
                if (s != "")
                {
                    string[] a = s.Split('.');
                    s = a[0] + '.' + "eyJpYXQiOjE1MzYwMTg3OTQsImlwIjoiMTgyLjEwOS41OC4xMDkiLCJ1YSI6MzgzODg2ODg0OH0" + '.' + a[2];

                    if (DownloadEngine.Servers.Bloodcat.IsCookieValid(s))
                    {
                        WriteStatus(Status.Ok);
                        Cookie = s;
                        return true;
                    }
                    else
                    {
                        WriteStatus(Status.Fail);
                    }
                }
                else
                {
                    WriteStatus(Status.Skip);
                    Cookie = null;
                    return false;
                }
            }
        }
        private static void WriteStatus(Status s)
        {
            string m = "[ {0} ]";
            Formatter f = null;
            switch (s)
            {
                case Status.Ok:
                    f = new Formatter("Ok", System.Drawing.Color.Green);
                    Console.SetCursorPosition(Console.WindowWidth - 8, Console.CursorTop - 1);
                    break;
                case Status.Fail:
                    f = new Formatter("Fail", System.Drawing.Color.Red);
                    Console.SetCursorPosition(Console.WindowWidth - 10, Console.CursorTop - 1);
                    break;
                case Status.Skip:
                    f = new Formatter("Skip", System.Drawing.Color.White);
                    Console.SetCursorPosition(Console.WindowWidth - 10, Console.CursorTop - 1);
                    break;
            }
            Console.WriteLineFormatted(m, f, System.Drawing.Color.Silver);
        }
    }
}
