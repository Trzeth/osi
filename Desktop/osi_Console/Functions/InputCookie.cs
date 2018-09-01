using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osi_Console.Functions
{
    static class InputCookieFunction
    {
        internal static bool InputInsoCookie(out string Cookie)
        {
            Console.WriteLine("请输入浏览器储存在 inso.link 域名下命名为 do_not_remove_this_0w0 的Cookie。");
            Console.WriteLine("或按 回车键 跳过。");
            while (true)
            {
                string s = Console.ReadLine();
                if (s != "")
                {
                    if (DownloadEngine.Servers.Inso.IsCookieValid(s))
                    {
                        Console.WriteLine("Cookie 有效。");
                        Cookie = s;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Cookie 无效，请重试。");
                    }
                }
                else
                {
                    Cookie = null;
                    return false;
                }
            }
        }
        internal static bool InputBloodcatCookie(out string Cookie)
        {
            Console.WriteLine("请输入浏览器储存在 bloodcat.com 域名下命名为 obm_human 的Cookie。");
            Console.WriteLine("或按 回车键 跳过。");
            while (true)
            {
                string s = Console.ReadLine();
                if (s != "")
                {
                    if (DownloadEngine.Servers.Bloodcat.IsCookieValid(s))
                    {
                        Console.WriteLine("Cookie 有效。");
                        Cookie = s;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Cookie 无效，请重试。");
                    }
                }
                else
                {
                    Cookie = null;
                    return false;
                }
            }
        }

    }
}
