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
            get { return _beatmapsetPackage; }
            set { _beatmapsetPackage = value; }
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

            _beatmapsetPackage = p;
        }
        public void Downloade()
        {
            while (true)
            {
                if (_beatmapsetPackage.Server == null)
                {
                    _beatmapsetPackage.Server = ChooseServer(_beatmapsetPackage);
                    //NoServerToChoose Exception
                }

                try
                {
                    byte[] data = null;
                    string fileName = null;

                    Servers.Server server = GetServer((Server)_beatmapsetPackage.Server);
                    data = server.Download(_beatmapsetPackage, out fileName);
                    string s = FileWriter(data, fileName);
                    _beatmapsetPackage.OnProgressChanged(new ProgressChangedEventArgs(100,"已写入文件"));
                    _beatmapsetPackage.OnWriteFileComplete(new BeatmapsetPackage.WriteFileCompletedArg(s));
                    break;
                }
                catch (Exception e)
                {
                    _beatmapsetPackage.FailedServerList.Add((Server)_beatmapsetPackage.Server);
                    _beatmapsetPackage.Server = null;
                }
                finally
                {
                    GC.Collect();
                }
            }
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
