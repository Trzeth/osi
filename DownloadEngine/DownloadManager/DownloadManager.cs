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
    internal class BeatmapsetPackage
    {
        internal Server Server { get; set; }
        internal int BeatmapsetId { get { return _beatmapsetId; } }

        int _beatmapsetId;
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

        int _beatmapsetId;
        public FailedBeatmapset(BeatmapsetPackage bp)
        {
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
        public static void Config(string insoCookie,string bloodcatCookie)
        {
            Inso.SetCookie(insoCookie);
            Bloodcat.SetCookie(bloodcatCookie);
            _isInsoValid = true;
            _isBloodcatValid = true;
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
                                if (_isInsoValid != true) throw new Exception();
                                data = Inso.Download(p, out fileName);
                                break;
                            case Server.Blooadcat:
                                if (_isBloodcatValid != true) throw new Exception();
                                data = Bloodcat.Downlaod(p, out fileName);
                                break;
                            case Server.Uugl:
                                if (_isUuglValid != true) throw new Exception();
                                data = Uugl.Download(p, out fileName);
                                break;
                        }
                        FileHelper.FileWrite(data, fileName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        FailedBeatmapset f = new FailedBeatmapset(p);
                        lock (_pendingList) { _pendingList.Add(f); }
                    }
                }
                catch(Exception e)
                {
                    lock (_failedList) { _failedList.Add(p as FailedBeatmapset); }
                }
            }
        }
    }
}
