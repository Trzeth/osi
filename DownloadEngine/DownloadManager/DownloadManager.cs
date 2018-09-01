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
        internal static Dictionary<Server, bool> IsServerVaild;
        internal static bool _isInsoValid;
        internal static bool _isBloodcatValid;
        internal static bool _isUuglValid = true;
        internal FileWriter FileWriter { get { return _fileWriter; } }

        //public static List<Beatmapset> BeatmapsetList;
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

        static int _maxDownloaderCount = 2;
        private FileWriter _fileWriter;
        public DownloadManager()
        {
            //if (BeatmapsetList == null)
            //{
            //    BeatmapsetList = new List<Beatmapset>();
            //}
            if (IsServerVaild == null)
            {
                IsServerVaild = new Dictionary<Server, bool>();
                foreach (Server server in Enum.GetValues(typeof(Server)))
                {
                    IsServerVaild.Add(server, false);
                }
            }
            if (DownloaderList == null)
            {
                DownloaderList = new List<Downloader>();
                CheckDownloaderQuantity();
            }
        }
        public void Config(string cookie,Server server)
        {
            bool isVaild = false;
            switch (server)
            {
                case Server.Inso:
                    isVaild = Inso.SetCookie(cookie);
                    break;
                case Server.Blooadcat:
                    isVaild = Bloodcat.SetCookie(cookie);
                    break;
            }
            IsServerVaild[server] = isVaild;
        }
        public void Config(System.Net.CookieCollection cookieCollection,Server server)
        {
            switch (server)
            {
                case Server.Inso:
                    Inso.SetCookie(cookieCollection);
                    break;
                case Server.Blooadcat:
                    if (Bloodcat.SetCookie(cookieCollection) != true) throw new CookieInvalid();
                    break;
            }
            IsServerVaild[server] = true;
        }
        public void Add(Beatmapset beatmapset,Server? server = null)
        {
            //BeatmapsetList.Add(beatmapset);
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
                for (int i = 0; i < DownloaderList.Count || _monitoringDownloaderCount >= PendingQueue.Count; i++)
                {
                    if (DownloaderList[i].IsMonitoring != true)
                    {
                        DownloaderList[i].Monitor();
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
                for (int i = 0; i < (_maxDownloaderCount - DownloaderList.Count); i++)
                {
                    DownloaderList.Add(new Downloader(this));
                }
            }
        }
    }
}
