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
        static Queue<BeatmapsetPackage> _pendingQueue;
        static List<Downloader> _downloaderList;
        static int _downloaderCount;
        static int _monitoringDownloader;
        bool _isMonitoring;

        internal static Queue<BeatmapsetPackage> PendingQueue { get { return _pendingQueue; } }
        internal static List<Downloader> DownloaderList
        {
            get { return _downloaderList; }
            set { _downloaderList = value; }
        }
        internal static int MonitoringDownloader { get { return _monitoringDownloader; } }
        internal bool IsMonitoring { get { return _isMonitoring; } }
        internal Downloader()
        {
            if (_downloaderList == null)
            {
                _downloaderList = new List<Downloader>();
            }
            if (_pendingQueue == null)
            {
                _pendingQueue = new Queue<BeatmapsetPackage>();
            }
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
                        Thread.CurrentThread.Abort();
                        break;
                    }
                    p = _pendingQueue.Dequeue();
                }

                bool _succeed = false;
                int i = 0;
                while (true)
                {
                    if (p.Server == Server.Unset)
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

                    Servers.Server server = GetServer(p.Server);
                    if (!DownloadManager.IsServerVaild[p.Server]) throw new ServerNotAvailable();
                    data = server.Download(p, out fileName);
                    FileHelper.FileWrite(data, fileName);

                    try
                    {
                        _succeed = true;
                    }
                    catch (Exception e)
                    {
                        p.FailedServerList.Add(p.Server);
                        p.Server = Server.Unset;
                    }
                    finally
                    {
                        GC.Collect();
                    }
                    if (_succeed)
                    {
                        break;
                    }
                }
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
                    return new Inso();
                case Server.Uugl:
                    return new Inso();
                default:
                    return new Inso();
            }
        }
    }
}
