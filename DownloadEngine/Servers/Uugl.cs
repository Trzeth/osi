using System;
using System.Collections.Generic;
using System.Linq;
using DownloadEngine;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DownloadEngine.DownloadManager;
using WebClient = DownloadEngine.DownloadManager.WebClient;

namespace DownloadEngine.Servers
{
    class Uugl
    {
        public static byte[] Download(BeatmapsetPackage p,out string fileName)
        {
            WebClient webclient = new WebClient();
            byte[] file = webclient.DownloadData(Path(p));
            WebHeaderCollection responseHeaders = webclient.ResponseHeaders;

            if (responseHeaders.Get("Content-Length") == null)
            {
                throw new Exception("File Does't Exist");
            }

            fileName = WebClient.GetFileNameFromHeader(responseHeaders.Get("Content-Disposition"));

            webclient.Dispose();
            return file;
        }
        private static string Path(BeatmapsetPackage p)
        {
            const string uriRoot = "http://osu.uu.gl/";
            return uriRoot + "s/" + p.BeatmapsetId;
        }
    }
}
