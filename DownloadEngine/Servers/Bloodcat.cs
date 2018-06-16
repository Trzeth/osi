using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;

namespace DownloadEngine.Servers
{
    static class Bloodcat
    {
        private static CookieCollection _cookieCollection;
        internal static byte[] Downlaod(Beatmapset beatmapset,out string fileName)
        {
            if(_cookieCollection == null)
            {
                throw new Exception("Bloodcat No Cookie Existed.");
            }

            WebClient webclient = new WebClient();
            byte[] file;

            webclient.AddCookie(_cookieCollection);
            file = webclient.DownloadData(Path(beatmapset));
            
            fileName = WebClient.GetFileNameFromHeader(webclient.ResponseHeaders.Get("Content-Disposition"));
            webclient.Dispose();

            return file;
        }
        public struct CAPTCHAData
        {
            public string response;
            public string sync;
            public string hash;
        }
        public static string GetCAPTCHA(Uri uri,out CAPTCHAData data)
        {
            string image;
            string sync;
            string hash;

            Regex r_image = new Regex("<img src="+ @"\" + '"' + "data:image/jpeg;base64,(?<image>[^.]*)" + @"\" + '"' + " "+ "class="+ @"\" + '"' +"d-block mw-100"+ @"\" + '"' +">");
            //<img src=\"(?<image>[^.]*)\" class=\"d-block mw-100\">
            Regex r_sync = new Regex("<input name=" + @"\" + '"' + "sync" + @"\" + '"' + " " + "type=" + @"\" + '"' + "hidden" + @"\" + '"' + " " + "value=" + @"\" + '"' + @"(?<sync>\d+)" + @"\" + '"' + ">");
            //<input name=\"sync\" type=\"hidden\" value=\"(?<sync>\d+)\">
            Regex r_hash = new Regex("<input name=" + @"\" + '"' + "hash" + @"\" + '"' + " " + "type=" + @"\" + '"' + "hidden" + @"\" + '"' + " " + "value=" + @"\" + '"' + "(?<hash>[^" + '"' +"]*)" + @"\" + '"' + ">");
            //<input name=\"sync\" type=\"hidden\" value=\"(?<hash>[^"]*)\">

            WebClient webclient = new WebClient();
            string CAPTCHAPage = null;
            try
            {
                CAPTCHAPage = webclient.DownloadString(uri);
            }catch(WebException e)
            {
                StreamReader sR = new StreamReader(e.Response.GetResponseStream());
                CAPTCHAPage = sR.ReadToEnd();
            }

            Match m_image = r_image.Match(CAPTCHAPage);
            Match m_sync = r_sync.Match(CAPTCHAPage);
            Match m_hash = r_hash.Match(CAPTCHAPage);

            image = m_image.Groups["image"].Value;
            sync = m_sync.Groups["sync"].Value;
            hash = m_hash.Groups["hash"].Value;

            if(image == string.Empty | sync == string.Empty || hash == string.Empty)
            {
                throw new Exception("Failed Get CAPTCHAdata");
            }
            else
            {
                data = new CAPTCHAData();
                data.sync = sync;
                data.hash = hash;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(image));
        }
        public static void PostCAPTCHA(Uri uri,CAPTCHAData data)
        {
            string formData = "response" + data.response + "&" + "sync" + data.sync + "&" + "hash" + data.hash ;
            WebClient webclient = new WebClient();
            webclient.Referer = uri.ToString();
            webclient.UploadString(uri,formData);
            _cookieCollection = WebClient.GetAllCookiesFromHeader(webclient.ResponseHeaders.Get("Set-Cookie"),uri.Host.ToString());
        }
        internal static void AddSetting()
        {

        }
        private static string Path(Beatmapset b)
        {
            const string uriRoot = "http://bloodcat.com/osu/";
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
