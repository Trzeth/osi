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
    public enum DownloadStatus
    {
        Downloading,
        Pause,
        Stop,
        Failed
    }
    public class BeatmapsetPackage
    {
        public Beatmapset Beatmapset { get; set; }
        public DownloadStatus Status { get; set; }
        internal Server Server { get; set; }
        internal List<Server> FailedServerList;
        internal int BeatmapsetId { get { return _beatmapsetId; } }

        protected int _beatmapsetId;
        internal BeatmapsetPackage() { }
        internal BeatmapsetPackage(Beatmapset beatmapset, Server server)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
            Server = server;
            FailedServerList = new List<Server>();
        }
        internal BeatmapsetPackage(Beatmapset beatmapset)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
            Beatmapset = beatmapset;
            Server = Server.Unset;
            FailedServerList = new List<Server>();
        }
    }
    public class DownloadManager
    {
        internal static Dictionary<Server, bool> IsServerVaild;
        internal static bool _isInsoValid;
        internal static bool _isBloodcatValid;
        internal static bool _isUuglValid = true;

        public static List<Beatmapset> BeatmapsetList;
        public static int MaxDownloaderCount
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
        public DownloadManager()
        {
            if (BeatmapsetList == null)
            {
                BeatmapsetList = new List<Beatmapset>();
            }
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
        public void Add(Beatmapset beatmapset,Server server = Server.Unset)
        {
            BeatmapsetList.Add(beatmapset);
            PendingQueue.Enqueue(new BeatmapsetPackage(beatmapset, server));
            CheckDownloadersState();
        }
        private static void CheckDownloadersState()
        {
            //
            if (MonitoringDownloader < _maxDownloaderCount)
            {
                int _monitoringDownloaderCount = 0;
                for (int i = 0; i < DownloaderList.Count; i++)
                {
                    if (DownloaderList[i].IsMonitoring != true)
                    {
                        DownloaderList[i].Monitor();
                        _monitoringDownloaderCount++;
                    }
                    if (_monitoringDownloaderCount >= PendingQueue.Count)
                    {
                        break;
                    }
                }
            }
        }
        private static void CheckDownloaderQuantity()
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
                    DownloaderList.Add(new Downloader());
                    int a;
                }
            }
        }
    }
}
