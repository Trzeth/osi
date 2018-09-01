using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //public DownloadStatus Status { get; set; }
        internal Server? Server { get; set; }
        internal List<Server> FailedServerList { get; set; }
        internal int BeatmapsetId { get { return _beatmapsetId; } }

        protected int _beatmapsetId;
        public BeatmapsetPackage() { }
        public BeatmapsetPackage(Beatmapset beatmapset, Server? server)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
            Server = server;
            FailedServerList = new List<Server>();
        }
        public BeatmapsetPackage(Beatmapset beatmapset)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
            Beatmapset = beatmapset;
            Server = null;
            FailedServerList = new List<Server>();
        }

        public class BeatmapsetInfo : EventArgs
        {
            public int beatmapsetId;
            public string creator;
            public string title;
            public string artist;
        }
        public event EventHandler<BeatmapsetInfo> GetInfoCompleted;
        internal void OnGetInfoCompleted(BeatmapsetInfo e)
        {
            GetInfoCompleted(this, e);
        }

        public class DownloadProgressChangedArgs:EventArgs
        {
            public string Status;
        }
        public event EventHandler<DownloadProgressChangedArgs> DownloadProgressChanged;
        internal void OnDownloadProgressChanged(DownloadProgressChangedArgs e)
        {
            DownloadProgressChanged(this,e);
        }
    }
}
