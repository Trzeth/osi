using System;
using System.Collections.Generic;
using System.Linq;
using DownloadEngine;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DownloadEngine.DownloadManager;

namespace DownloadEngine.Servers
{
    class Uugl
    {
        public static byte[] Download(BeatmapsetPackage p,out string fileName)
        {
            WebClient webclient = new WebClient();
            byte[] file = webclient.DownloadData(Path(p));
            WebHeaderCollection responseHeaders = webclient.ResponseHeaders;

            if (int.Parse(responseHeaders.Get("Content-Length")) <= 0)
            {
                throw new Exception("File Does't Exist");
            }

            fileName = WebClient.GetFileNameFromHeader(responseHeaders.Get("Content-Disposition"));

            webclient.Dispose();
            GC.Collect();

            return file;
        }
        private static string Path(BeatmapsetPackage p)
        {
            const string uriRoot = "http://osu.uu.gl/";
            return uriRoot + "s/" + p.BeatmapsetId;
        }
    }
}
