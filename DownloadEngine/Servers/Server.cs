using DownloadEngine.DownloadManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadEngine.Servers
{
    abstract class Server
    {
        class ReturnInformation { }
        internal abstract byte[] Download(BeatmapsetPackage p, out string fileName);
    }
    interface IServer
    {
    }
}
