using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadEngine.Servers;
using static DownloadEngine.DownloadManager.DownloadHepler;
using static DownloadEngine.DownloadManager.Downloader;
using System.Threading;

namespace DownloadEngine.DownloadManager
{
    public class DownloadManager
    {
        internal static Dictionary<Server, bool> IsServerVaild = new Dictionary<Server, bool> ()
        {
            { Server.Inso,false },
            { Server.Blooadcat,false},
            { Server.Orgin,false}
        };
        public FileWriter FileWriter
        {
            get { return _fileWriter; }
            set { _fileWriter = value; }
        }

        public int MaxDownloaderCount
        {
            get { return _maxDownloaderCount; }
            set
            {
                if (value > 0)
                {
                    _maxDownloaderCount = value;
                    CheckDownloaderQuantity();
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        private static int _maxDownloaderCount = 2;
        private FileWriter _fileWriter;
        public DownloadManager() { }
        public bool Config(string cookie, Server server, bool skipVerification = false)
        {
            if (!skipVerification)
            {
                bool isVaild = false;
                switch (server)
                {
                    case Server.Inso:
                        isVaild = Inso.IsCookieValid(cookie);
                        if(isVaild) Inso.SetCookie(cookie);
                        break;
                    case Server.Blooadcat:
                        isVaild = Bloodcat.IsCookieValid(cookie);
                        if (isVaild) Bloodcat.SetCookie(cookie);
                        break;
                }
                IsServerVaild[server] = isVaild;
                return isVaild;
            }
            else
            {
                switch (server)
                {
                    case Server.Inso:
                        Inso.SetCookie(cookie);
                        break;
                    case Server.Blooadcat:
                        Bloodcat.SetCookie(cookie);
                        break;
                }
                IsServerVaild[server] = true;
                return true;
            }
        }
        public void Add(Beatmapset beatmapset,Server? server = null)
        {
            Add(new BeatmapsetPackage(beatmapset, server));
        }
        public void Add(BeatmapsetPackage beatmapsetPackage)
        {
            PendingQueue.Enqueue(beatmapsetPackage);
            CheckDownloadersState();
        }
        private void CheckDownloadersState()
        {
            if (MonitoringDownloader < _maxDownloaderCount)
            {
                int _monitoringDownloaderCount = 0;
                for (int i = 0; i < DownloaderList.Count && _monitoringDownloaderCount < PendingQueue.Count; i++)
                {
                    if (DownloaderList[i].IsMonitoring != true)
                    {
                        Thread t = new Thread(new ThreadStart(delegate {
                            DownloaderList[i].Monitor();
                        }));
                        t.Start();
                        _monitoringDownloaderCount++;
                    }
                }
            }
        }
        private void CheckDownloaderQuantity()
        {
            if (DownloaderList.Count > _maxDownloaderCount)
            {
                for (int i = 0; i < (DownloaderList.Count - _maxDownloaderCount); i++)
                {
                    DownloaderList.Remove(DownloaderList[i]);
                }
            }
            else
            {
                int c = _maxDownloaderCount - DownloaderList.Count;
                for (int i = 0; i < c; i++)
                {
                    DownloaderList.Add(new Downloader(this));
                    Console.WriteLine(DownloaderList.Count());
                }
            }
        }
    }
}
