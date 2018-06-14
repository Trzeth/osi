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
        public static byte[] Download(Beatmapset Beatmapset,out string Filename)
        {
            WebClient webclient = new WebClient();
            byte[] file = webclient.DownloadData(Path(Beatmapset));
            WebHeaderCollection responseHeaders = webclient.ResponseHeaders;

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create();
            //request.UserAgent = DownloadManager.UserAgent;
            //request.Method = "Get";
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (int.Parse(responseHeaders.Get("Content-Length")) <= 0)
            {
                throw new Exception("File Does't Exist");
            }

            //Get File Name
            Regex FilenameRegex = new Regex(@"\" + '"' + @"(?<Filename>[^" + '"' + "]*)" + @"\" + '"');
            // \"(?<a>[^"]*)\"
            // '"' = \"
            if (FilenameRegex.IsMatch(responseHeaders.Get("Content-Disposition")))
            {
                Match valve = FilenameRegex.Match(responseHeaders.Get("Content-Disposition"));
                Filename = valve.Groups["Filename"].Value;
            }
            else
            {
                throw new Exception("No Match Head");
            }

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
