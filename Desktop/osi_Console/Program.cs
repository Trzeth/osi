using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DownloadEngine;
using DownloadEngine.Servers;
using DownloadEngine.DownloadManager;
using Server = DownloadEngine.Server;
using osi_Console.Functions;

namespace osi_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "osi_Console";
            Console.WriteLine("START");

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appsettings = config.AppSettings.Settings;

            string InsoCookie;
            string BloodcatCookie;

            if (appsettings["FirstRun"].Value == "true")
            {
                string httpHash, httpsHash;

                RegisterAsDefaultBrowserFunction.Register(out httpHash,out httpsHash);
                if (httpHash != null)
                {
                    appsettings["HttpHash"].Value = httpHash;
                }
                if (httpsHash != null)
                {
                    appsettings["HttpsHash"].Value = httpsHash;
                }

                while (!(InputCookieFunction.InputInsoCookie(out InsoCookie) | InputCookieFunction.InputBloodcatCookie(out BloodcatCookie)))
                {
                    Console.WriteLine("请至少满足一个服务器的需求。");
                }
                if (InsoCookie != null)
                {
                    appsettings["InsoCookie"].Value = InsoCookie;
                }
                if (BloodcatCookie != null)
                {
                    appsettings["BloodcatCookie"].Value = BloodcatCookie;
                }
                appsettings["FirstRun"].Value = "false";
            }
            else
            {
                InsoCookie = appsettings["InsoCookie"].Value;
                BloodcatCookie = appsettings["BloodcatCookie"].Value;
                if (!Inso.IsCookieValid(InsoCookie))
                {
                    Console.WriteLine("Inso Cookie已失效 请重新输入。");
                    InputCookieFunction.InputInsoCookie(out InsoCookie);
                    appsettings["InsoCookie"].Value = InsoCookie;
                }
                else
                {
                    Console.WriteLine("do_not_remove_this_0w0" + " = " + InsoCookie);
                    Console.WriteLine("Inso Cookie有效。");
                }
                if (!Bloodcat.IsCookieValid(BloodcatCookie))
                {
                    Console.WriteLine("Bloodcat Cookie已失效 请重新输入。");
                    InputCookieFunction.InputBloodcatCookie(out BloodcatCookie);
                    appsettings["BloodcatCookie"].Value = BloodcatCookie;
                }
                else
                {
                    Console.WriteLine("obm_human" + " = " + BloodcatCookie);
                    Console.WriteLine("Bloodcat Cookie有效。");
                }
            }
            config.Save();

            DownloadManager downloadMgr = new DownloadManager();

            downloadMgr.Config(InsoCookie, Server.Inso);
            downloadMgr.Config(BloodcatCookie,Server.Blooadcat);
            downloadMgr.MaxDownloaderCount = 2;

            while (true)
            {
                BeatmapsetPackage p = new BeatmapsetPackage(new Beatmapset("https://osu.ppy.sh/beatmapsets/744238"), Server.Inso);
                p.GetInfoCompleted += new EventHandler<BeatmapsetPackage.BeatmapsetInfo>(delegate (object e, BeatmapsetPackage.BeatmapsetInfo info)
                {
                    Console.WriteLine(info.beatmapsetId + " " + info.artist + "-" + info.title);
                });
                downloadMgr.Add(p);
            }
            Console.ReadLine();
        }
    }
}
