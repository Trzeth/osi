using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadEngine.DownloadManager;
using DownloadEngine.Servers;
using static DownloadEngine.DownloadManager.DownloadHepler;

namespace DownloadEngine
{
    public class Downloader
    {
        public BeatmapsetPackage BeatmapsetPackage
        {
            get
            {
                return _beatmapsetPackage;
            }
            set
            {
                _beatmapsetPackage = value;
                Server = _beatmapsetPackage.Server;
            }
        }
        private BeatmapsetPackage _beatmapsetPackage;
        public FileWriter FileWriter {
            get {
                if (_fileWriter != null) return _fileWriter;
                else return FileHelper.FileWrite;
                 }
            set { _fileWriter = value; }
        }
        private FileWriter _fileWriter;

        public Server Server
        {
            get
            {
                //NoServerChoose Exception
                if (_server == Server.Unset)
                {
                    Server = ChooseServer(BeatmapsetPackage);
                    return _server;
                }
                else return _server;
            }
            set
            {
                _server = value;
                switch (_server)
                {
                    case Server.Inso:
                        _serverClass = new Inso(BeatmapsetPackage);
                        break;
                    case Server.Blooadcat:
                        _serverClass = new Bloodcat(BeatmapsetPackage);
                        break;
                    default:
                        _serverClass = null;
                        break;
                }

            }
        }
        private Server _server;
        private Servers.Server _serverClass;

        private byte[] _data;

        public Downloader() { }
        public Downloader(BeatmapsetPackage p,FileWriter fileWriter=null)
        {
            if (fileWriter != null)
            {
                this.FileWriter = fileWriter;
            }
            else
            {
                this.FileWriter = FileHelper.FileWrite;
            }

            BeatmapsetPackage = p;
        }
        public void Download()
        {
            GetInformation();
            DownloadData();
            WriteToFile();
        }
        public BeatmapsetInfo GetInformation()
        {
            if (_beatmapsetPackage == null)
            {
                throw new Exception("Beatmapset didn't set.");
            }

            return _serverClass.GetInformation();
        }
        public void DownloadData()
        {
            if (_beatmapsetPackage == null)
            {
                throw new Exception("Beatmapset didn't set.");
            }

            while (true)
            {
                try
                {
                    _data = null;
                    _data = _serverClass.Download();
                    _beatmapsetPackage.OnProgressChanged(new ProgressChangedEventArgs(100,"下载完成"));
                    break;
                }
                catch (Exception e)
                {
                    _beatmapsetPackage.FailedServerList.Add(_beatmapsetPackage.Server);
                    Server = Server.Unset;
                }
            }
            GC.Collect();
        }
        public void WriteToFile()
        {
            if(_data==null)throw new Exception("data为Null 无法写入");
            string s = FileWriter(_data, _serverClass.FileName);
            _beatmapsetPackage.OnWriteFileComplete(new BeatmapsetPackage.WriteFileCompletedArg(s));
        }
    }
}
