using System;
using System.Collections.Generic;
using DownloadEngine.Servers;
using System.Threading;
using static DownloadEngine.DownloadManager.DownloadHepler;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadEngine.DownloadManager
{
    class Downloader
    {
        static Queue<BeatmapsetPackage> _pendingQueue = new Queue<BeatmapsetPackage>();
        static List<Downloader> _downloaderList = new List<Downloader>();
        static int _downloaderCount;
        static int _monitoringDownloader;
        bool _isMonitoring;

        private DownloadManager _downloadMgr;
        internal static Queue<BeatmapsetPackage> PendingQueue { get { return _pendingQueue; } }
        internal static List<Downloader> DownloaderList
        {
            get { return _downloaderList; }
            set { _downloaderList = value; }
        }
        internal static int MonitoringDownloader { get { return _monitoringDownloader; } }
        internal bool IsMonitoring { get { return _isMonitoring; } }
        internal Downloader(DownloadManager DownloadManager)
        {
            _downloadMgr = DownloadManager;
            _isMonitoring = false;
        }
        internal void Monitor()
        {
            _isMonitoring = true;
            _monitoringDownloader++;

            while (true)
            {
                BeatmapsetPackage p;
                lock (_pendingQueue)
                {
                    if (_pendingQueue.Count == 0)
                    {
                        _downloaderCount--;
                        GC.Collect();
                        break;
                    }
                    p = _pendingQueue.Dequeue();
                }

                while (true)
                {
                    if (p.Server == null)
                    {
                        try
                        {
                            p.Server = ChooseServer(p);
                        }
                        catch (NoServerToChoose e)
                        {
                            break;
                        }
                    }

                    byte[] data = null;
                    string fileName = null;

                    try
                    {
                        Servers.Server server = GetServer((Server)p.Server);
                        if (!DownloadManager.IsServerVaild[(Server)p.Server]) throw new ServerNotAvailable();
                        data = server.Download(p, out fileName);
                        string s = _downloadMgr.FileWriter(data, fileName);
                        p.OnWriteFileComplete(new BeatmapsetPackage.WriteFileCompletedArg(s));
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        p.FailedServerList.Add((Server)p.Server);
                        p.Server = null;
                    }
                }
                GC.Collect();
            }

            _isMonitoring = false;
            _monitoringDownloader--;
        }
        private Servers.Server GetServer(Server server)
        {
            switch (server)
            {
                case Server.Inso:
                    return new Inso();
                case Server.Blooadcat:
                    return new Bloodcat();
                default:
                    return null;
            }
        }
    }
}
