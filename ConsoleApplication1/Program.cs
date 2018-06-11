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

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appsettings = config.AppSettings.Settings;

            if (appsettings["Inso"].Value == String.Empty)
            {
                Console.WriteLine("Input Inso Cookie");
                appsettings["Inso"].Value = Console.ReadLine();
                config.Save();
            }

            DownloadEngine.Servers.Inso.Cookie = ConfigurationManager.AppSettings["Inso"];

            while (true)
            {
                Console.WriteLine("Input Uri");
                try
                {
                    Beatmapset beatmapset = new Beatmapset();
                    beatmapset.Uri = new Uri(Console.ReadLine());
                    Downloader downloader = new Downloader(beatmapset);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
