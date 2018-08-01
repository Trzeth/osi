using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadEngine.Servers;
using static DownloadEngine.DownloadManager._DownlaodHepler;
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
        public DownloadStatus Status;
        internal Server Server { get; set; }
        internal List<Server> FailedServers;
        internal int BeatmapsetId { get { return _beatmapsetId; } }

        protected int _beatmapsetId;
        internal BeatmapsetPackage() { }
        internal BeatmapsetPackage(Beatmapset beatmapset, Server server)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
            Server = server;
        }
        internal BeatmapsetPackage(Beatmapset beatmapset)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
            Server = Server.Unset;
        }
    }
    internal class FailedBeatmapset : BeatmapsetPackage
    {
        internal List<Server> FailedServerList { get; set; }
        public FailedBeatmapset(BeatmapsetPackage bp)
        {
            FailedServerList = new List<Server>();
            FailedServerList.Add(bp.Server);
            Server = Server.Unset;

            _beatmapsetId = bp.BeatmapsetId;
        }
        public FailedBeatmapset(FailedBeatmapset bp)
        {
            FailedServerList = bp.FailedServerList;
            FailedServerList.Add(bp.Server);
            Server = Server.Unset;

            _beatmapsetId = bp.BeatmapsetId;
        }
    }
    public class DownloadManager
    {        
        static bool _isInsoValid;
        static bool _isBloodcatValid;
        static bool _isUuglValid = true;

        public List<BeatmapsetPackage> BeatmapsetList = new List<BeatmapsetPackage>();
        static Queue<BeatmapsetPackage> _pendingQueue = new Queue<BeatmapsetPackage>();
        static List<BeatmapsetPackage> _pendingList = new List<BeatmapsetPackage>();
        static List<FailedBeatmapset> _failedList = new List<FailedBeatmapset>();
        static int _downloaderCount;
        public static int MaxDownloaderCount
        {
            get { return _maxDownloaderCount; }
            set
            {
                if (value > 0)
                {
                    _maxDownloaderCount = value;
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        static int _maxDownloaderCount = 2;
        public DownloadManager() { }
        public static void Config(string cookie,Server server)
        {
            switch (server)
            {
                case Server.Inso:
                    Inso.SetCookie(cookie);
                    _isInsoValid = true;
                    break;
                case Server.Blooadcat:
                    Bloodcat.SetCookie(cookie);
                    _isBloodcatValid = true;
                    break;
            }
        }
        public static void Config(System.Net.CookieCollection cookieCollection,Server server)
        {
            switch (server)
            {
                case Server.Inso:
                    Inso.SetCookie(cookieCollection);
                    _isInsoValid = true;
                    break;
                case Server.Blooadcat:
                    if (Bloodcat.SetCookie(cookieCollection) != true) throw new CookieInvalid();
                    _isBloodcatValid = true;
                    break;
            }
        }
        public static void Add(Beatmapset beatmapset,Server server)
        {
            BeatmapsetPackage beatmapsetPackage = new BeatmapsetPackage(beatmapset,server);
            _pendingList.Add(beatmapsetPackage);
            Download();
        }
        public static void Add(Beatmapset beatmapset)
        {
            BeatmapsetPackage beatmapsetPackage = new BeatmapsetPackage(beatmapset);
            _pendingList.Add(beatmapsetPackage);
            Download();
        }
        private static void Download()
        {
            if (_downloaderCount < _maxDownloaderCount)
            {
                int c;
                if (_pendingList.Count < _maxDownloaderCount)
                {
                    c = _pendingList.Count;
                }
                else
                {
                    c = _maxDownloaderCount;
                }
                for (int i = 0; i < (c - _downloaderCount); i++)
                {
                    Thread thread = new Thread(new ParameterizedThreadStart(Downloader));
                    thread.Start();
                    _downloaderCount++;
                }
            }
        }
        private static void Downloader(object o)
        {
            while (true)
            {
                BeatmapsetPackage p;
                lock (_pendingList)
                {
                    if (_pendingList.Count == 0)
                    {
                        _downloaderCount--;
                        GC.Collect();
                        Thread.CurrentThread.Abort();
                        break;
                    }
                    p = _pendingList.First();
                    _pendingList.Remove(p);
                }
                try
                {
                    if (p is FailedBeatmapset)
                    {
                        p.Server = ChooseServer(p as FailedBeatmapset);
                    }
                    else if (p.Server == Server.Unset)
                    {
                        p.Server = ChooseServer(p);
                    }

                    byte[] data = null;
                    string fileName = null;
                    try
                    {
                        switch (p.Server)
                        {
                            case Server.Orgin:
                                break;
                            case Server.Inso:
                                if (_isInsoValid != true) throw new ServerNotAvailable();
                                data = Inso.Download(p, out fileName);
                                break;
                            case Server.Blooadcat:
                                if (_isBloodcatValid != true) throw new ServerNotAvailable();
                                data = Bloodcat.Downlaod(p, out fileName);
                                break;
                            case Server.Uugl:
                                if (_isUuglValid != true) throw new ServerNotAvailable();
                                data = Uugl.Download(p, out fileName);
                                break;
                        }
                        FileHelper.FileWrite(data, fileName);
                    }
                    catch (Exception e)
                    {
                        FailedBeatmapset f;
                        if (p is FailedBeatmapset)
                        {
                            f = new FailedBeatmapset(p as FailedBeatmapset);
                        }
                        else
                        {
                            f = new FailedBeatmapset(p as BeatmapsetPackage);
                        }
                        lock (_pendingList) { _pendingList.Add(f); }
                    }
                }
                catch (NoServerToChoose e)
                {
                    lock (_failedList) { _failedList.Add(p as FailedBeatmapset); }
                }
                finally
                {
                    GC.Collect();
                }
            }
        }
    }
}
