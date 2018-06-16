using System;
using System.Collections.Generic;
using System.Linq;
using DownloadEngine;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DownloadEngine.Servers
{
    class Uugl
    {
        public static byte[] Download(Beatmapset Beatmapset,out string fileName)
        {
            WebClient webclient = new WebClient();
            byte[] file = webclient.DownloadData(Path(Beatmapset));
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
        private static string Path(Beatmapset b)
        {
            const string uriRoot = "http://osu.uu.gl/";
            if (b.BeatmapId != 0)
            {
                return uriRoot + "b/" + b.BeatmapId;
            }
            else
            {
                return uriRoot + "s/" + b.BeatmapSetId;
            }

        }
    }
}
