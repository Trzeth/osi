using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadEngine.Servers
{
    interface _IServer
    {
         byte[] Download(Beatmapset b);

    }
}
