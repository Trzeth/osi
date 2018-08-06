using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DownloadEngine;
using DownloadEngine.Servers;
using DownloadEngine.DownloadManager;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("START");

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appsettings = config.AppSettings.Settings;

            DownloadManager downloadMgr = new DownloadManager();

            if (appsettings["Inso"].Value == string.Empty)
            {
                Console.WriteLine("Input Inso Cookie");
                appsettings["Inso"].Value = Console.ReadLine();
                config.Save();
            }

            downloadMgr.Config(ConfigurationManager.AppSettings["Inso"], Server.Inso);
            DownloadManager.MaxDownloaderCount = 2;

            //DownloadManager.Add(new Beatmapset("https://osu.ppy.sh/beatmapsets/765055"));
            //DownloadManager.Add(new Beatmapset("https://osu.ppy.sh/beatmapsets/766867"));
            downloadMgr.Add(new Beatmapset("https://osu.ppy.sh/beatmapsets/744238"),Server.Inso);

            Console.ReadLine();
            //Server server = Server.Uugl;
            //Console.WriteLine("Select A Download Server");
            //Console.WriteLine("A Uugl B Inso C Bloodcat");
            //switch (Console.ReadKey().Key)
            //{
            //    case ConsoleKey.A:
            //        server = Server.Uugl;
            //        break;
            //    case ConsoleKey.B:
            //        server = Server.Inso;
            //        break;
            //    case ConsoleKey.C:
            //        server = Server.Blooadcat;
            //        break;
            //}

            //Console.WriteLine();
            //Console.WriteLine("Press A to enter Beatmap ID");
            //Console.WriteLine("Press B to enter BeatmapSet ID");
            //Console.WriteLine("Press C to enter Beatmap Uri");

            //while (true)
            //{
            //    Beatmapset beatmapset = null;
            //    Uri uri;
            //    ConsoleKey consoleKey = Console.ReadKey().Key;
            //    Console.WriteLine();

            //    switch (consoleKey)
            //    {
            //        case (ConsoleKey.B):
            //            Console.WriteLine("Enter BeatmapSet ID");
            //            beatmapset = new Beatmapset(int.Parse(Console.ReadLine()));
            //            break;
            //        case (ConsoleKey.C):
            //            Console.WriteLine("Enter Beatmap Uri");

            //            try
            //            {
            //                uri = new Uri(Console.ReadLine());
            //            }
            //            catch (Exception e)
            //            {
            //                Console.WriteLine(e.Message);
            //                uri = null;
            //            }
            //            beatmapset = new Beatmapset(uri);
            //            break;
            //    }

            //    DownloadManager.Add(beatmapset,server);
            //    Console.WriteLine("Finished!");
        }
    }
}
