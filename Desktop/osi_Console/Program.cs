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
using System.IO.Pipes;
using System.IO;

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
                if (InsoCookie != null)
                {
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

                }
                if (BloodcatCookie != null)
                {
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

            }
            config.Save();

            DownloadManager downloadMgr = new DownloadManager();

            downloadMgr.Config(InsoCookie, Server.Inso,true);
            downloadMgr.Config(BloodcatCookie,Server.Blooadcat,true);
            downloadMgr.MaxDownloaderCount = 2;
            downloadMgr.FileWriter = DownloadHepler.FileHelper.FileWrite;

            Console.WriteLine("OK.Start Monitor.");
            NamedPipeServerStream server = new NamedPipeServerStream("osi", PipeDirection.In);

            while (true)
            {
                server.WaitForConnection();

                StreamReader sr = new StreamReader(server);
                string link = sr.ReadToEnd();
                Console.WriteLine("Get Link" + " " + link);

                BeatmapsetPackage package = new BeatmapsetPackage(new Beatmapset(link),Server.Inso);
                package.GetInfoCompleted += delegate(object sender,BeatmapsetPackage.BeatmapsetInfo e) 
                {
                    Console.WriteLine(e.beatmapsetId + " " + e.artist + "-" + e.title);
                };
                package.DownloadProgressChanged += delegate(object sender, BeatmapsetPackage.DownloadProgressChangedArgs e)
                {
                    Console.WriteLine(e.Status);
                };
                package.WriteFileCompleted += delegate (object sender, BeatmapsetPackage.WriteFileCompletedArg e)
                {
                    System.Diagnostics.Process.Start(e.Path);
                };
                downloadMgr.Add(package);
                server.Disconnect();
            }
        }
    }
}
