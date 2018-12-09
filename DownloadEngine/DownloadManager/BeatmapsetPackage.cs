using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        internal Server Server { get; set; }
        internal List<Server> FailedServerList { get; set; }
        internal int BeatmapsetId { get { return _beatmapsetId; } }

        protected int _beatmapsetId;
        public BeatmapsetPackage() { }
        public BeatmapsetPackage(Beatmapset beatmapset, Server server)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
            Server = server;
            FailedServerList = new List<Server>();
        }
        public BeatmapsetPackage(Beatmapset beatmapset)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
            Beatmapset = beatmapset;
            Server = Server.Unset;
            FailedServerList = new List<Server>();
        }

        public event ProgressChangedEventHandler ProgressChanged;
        public void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null) ProgressChanged(this, e);
        }

        public class WriteFileCompletedArg : EventArgs
        {
            public string Path;
            public WriteFileCompletedArg() { }
            public WriteFileCompletedArg(string s)
            {
                Path = s;
            }
        }
        public event EventHandler<WriteFileCompletedArg> WriteFileCompleted;
        internal void OnWriteFileComplete(WriteFileCompletedArg e)
        {
            WriteFileCompleted(this,e);
        }
    }
}
