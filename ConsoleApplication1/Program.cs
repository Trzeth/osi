using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DownloadEngine;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("START");

            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var appsettings = config.AppSettings.Settings;

            //if (appsettings["Inso"].Value == string.Empty)
            //{
            //    Console.WriteLine("Input Inso Cookie");
            //    appsettings["Inso"].Value = Console.ReadLine();
            //    config.Save();
            //}

            DownloadManager DownloadMgr = new DownloadManager(ConfigurationManager.AppSettings["Inso"]);


            Server server = Server.Uugl;
            Console.WriteLine("Select A Download Server");
            Console.WriteLine("A Uugl B Inso C Bloodcat");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.A:
                    server = Server.Uugl;
                    break;
                case ConsoleKey.B:
                    server = Server.Inso;
                    break;
                case ConsoleKey.C:
                    server = Server.BlooadCat;
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("Press A to enter Beatmap ID");
            Console.WriteLine("Press B to enter BeatmapSet ID");
            Console.WriteLine("Press C to enter Beatmap Uri");

            while (true)
            {
                Beatmapset beatmapset = null;
                Uri uri;
                ConsoleKey consoleKey = Console.ReadKey().Key;
                Console.WriteLine();

                switch (consoleKey)
                {
                    case (ConsoleKey.A):
                        Console.WriteLine("Enter Beatmap ID");
                        beatmapset = new Beatmapset(int.Parse(Console.ReadLine()), IdType.BeatmapId, server);
                        break;
                    case (ConsoleKey.B):
                        Console.WriteLine("Enter BeatmapSet ID");
                        beatmapset = new Beatmapset(int.Parse(Console.ReadLine()), IdType.BeatmapSetId, server);
                        break;
                    case (ConsoleKey.C):
                        Console.WriteLine("Enter Beatmap Uri");

                        try
                        {
                            uri = new Uri(Console.ReadLine());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            uri = null;
                        }
                        beatmapset = new Beatmapset(uri, server);
                        break;
                }

                DownloadManager.AddToDownloadList(beatmapset);
                Console.WriteLine("Finished!");
            }
        }
    }
}
