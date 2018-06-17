using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using DownloadEngine.DownloadManager;

namespace DownloadEngine.Servers
{
    public static class Bloodcat
    {
        internal static CookieCollection _cookieCollection;
        internal static byte[] Downlaod(BeatmapsetPackage p,out string fileName)
        {
            if(_cookieCollection == null)
            {
                throw new Exception("Bloodcat No Cookie Existed.");
            }

            WebClient webclient = new WebClient();
            byte[] file;

            webclient.AddCookie(_cookieCollection);
            file = webclient.DownloadData(Path(p));
            
            fileName = WebClient.GetFileNameFromHeader(webclient.ResponseHeaders.Get("Content-Disposition"));
            webclient.Dispose();

            return file;
        }
        internal static void AddSetting()
        {

        }
        internal static void SetCookie(string cookieString)
        {
            if(_cookieCollection == null)
            {
                _cookieCollection = new CookieCollection();
            }

            Cookie cookie = new Cookie();
            cookie.Name = "obm_human";
            cookie.Path = "/osu/";
            cookie.Domain = "bloodcat.com";

            _cookieCollection.Add(cookie);
        }
        #region Search
        //internal static JObject Search()
        //{

        //}
        //private static JObject Search()
        //{
        //    WebClient webclient = new WebClient();
        //    webclient.DownloadString(Path("a"));
        //}
        #endregion
        #region CAPTCHA
        public struct CAPTCHAData
        {
            public string sync;
            public string hash;
            public Uri uri;
        }
        public static byte[] GetCAPTCHA(Uri uri, out CAPTCHAData data)
        {
            string image;
            string sync;
            string hash;

            Regex r_image = new Regex("<img src=" + @"\" + '"' + "data:image/jpeg;base64,(?<image>[^.]*)" + @"\" + '"' + " " + "class=" + @"\" + '"' + "d-block mw-100" + @"\" + '"' + ">");
            //<img src=\"(?<image>[^.]*)\" class=\"d-block mw-100\">
            Regex r_sync = new Regex("<input name=" + @"\" + '"' + "sync" + @"\" + '"' + " " + "type=" + @"\" + '"' + "hidden" + @"\" + '"' + " " + "value=" + @"\" + '"' + @"(?<sync>\d+)" + @"\" + '"' + ">");
            //<input name=\"sync\" type=\"hidden\" value=\"(?<sync>\d+)\">
            Regex r_hash = new Regex("<input name=" + @"\" + '"' + "hash" + @"\" + '"' + " " + "type=" + @"\" + '"' + "hidden" + @"\" + '"' + " " + "value=" + @"\" + '"' + "(?<hash>[^" + '"' + "]*)" + @"\" + '"' + ">");
            //<input name=\"sync\" type=\"hidden\" value=\"(?<hash>[^"]*)\">

            WebClient webclient = new WebClient();
            string CAPTCHAPage = null;
            try
            {
                CAPTCHAPage = webclient.DownloadString(uri);
            }
            catch (WebException e)
            {
                StreamReader sR = new StreamReader(e.Response.GetResponseStream());
                CAPTCHAPage = sR.ReadToEnd();
            }
            webclient.Dispose();

            Match m_image = r_image.Match(CAPTCHAPage);
            Match m_sync = r_sync.Match(CAPTCHAPage);
            Match m_hash = r_hash.Match(CAPTCHAPage);

            image = m_image.Groups["image"].Value;
            sync = m_sync.Groups["sync"].Value;
            hash = m_hash.Groups["hash"].Value;

            if (image == string.Empty | sync == string.Empty || hash == string.Empty)
            {
                throw new Exception("Failed Get CAPTCHAdata");
            }
            else
            {
                data = new CAPTCHAData();
                data.sync = sync;
                data.hash = hash;
                data.uri = uri;
            }
            return Convert.FromBase64String(image);
        }
        public static CookieCollection PostCAPTCHA(string response, CAPTCHAData data)
        {
            string formData = "response=" + response + "&" + "sync=" + data.sync + "&" + "hash=" + data.hash;
            WebClient webclient = new WebClient();
            webclient.Referer = data.uri.ToString();
            webclient.Host = data.uri.Host;
            webclient.ContentType = "application/x-www-form-urlencoded";
            webclient.UploadString(data.uri, formData);
            string Header = webclient.ResponseHeaders.Get("Set-Cookie");
            webclient.Dispose();

            return WebClient.GetAllCookiesFromHeader(Header, data.uri.Host.ToString());
        }
        #endregion
        private static string Path(BeatmapsetPackage p)
        {
            const string uriRoot = "http://bloodcat.com/osu/";

            return uriRoot + "s/" + p.BeatmapsetId;
        }
        //private static string Path(string q,)
        //{


        //}
    }
}
