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
using System.Windows.Media.Imaging;
//using WebClient = DownloadEngine.DownloadManager.WebClient;

namespace DownloadEngine.Servers
{
    public static class Bloodcat
    {
        static CookieCollection _cookieCollection;
        static DownloadManager.WebClient _webclient;
        internal static byte[] Downlaod(BeatmapsetPackage p,out string fileName)
        {
            if(_cookieCollection == null)
            {
                throw new Exception("Bloodcat No Cookie Existed.");
            }

            byte[] file;

            file = WebClient().DownloadData(Path(p));
            
            fileName = DownloadManager.WebClient.GetFileNameFromHeader(WebClient().ResponseHeaders.Get("Content-Disposition"));

            return file;
        }
        internal static void AddSetting()
        {

        }
        internal static bool SetCookie(string cookieString)
        {
            CookieCollection cookieCollection = new CookieCollection();

            Cookie cookie = new Cookie();
            cookie.Name = "obm_human";
            cookie.Path = "/osu/";
            cookie.Domain = "bloodcat.com";

            cookieCollection.Add(cookie);
            if (IsCookieValid(cookieCollection))
            {
                _cookieCollection = cookieCollection;
                return true;
            }
            else
            {
                return false;
            }
        }
        internal static bool SetCookie(CookieCollection cookieCollection)
        {
            if (IsCookieValid(_cookieCollection))
            {
                if (_cookieCollection == null)
                {
                    _cookieCollection = cookieCollection;
                }
                else
                {
                    _cookieCollection.Add(cookieCollection);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool IsCookieValid(CookieCollection cookieCollectioin)
        {
            WebClient(cookieCollectioin).Method = "Head";
            try
            {
                WebClient(cookieCollectioin).DownloadData("http://bloodcat.com/osu/s/4079");
                return true;
            }
            catch(WebException e)
            {
                return false;
            }
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
        public static BitmapImage GetCAPTCHA(Uri uri, out CAPTCHAData data)
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

            string CAPTCHAPage = null;
            try
            {
                CAPTCHAPage = WebClient().DownloadString(uri);
            }
            catch (WebException e)
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

            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(Convert.FromBase64String(image));
            bitmapImage.EndInit();

            return bitmapImage;
        }
        public static CookieCollection PostCAPTCHA(string response, CAPTCHAData data)
        {
            string formData = "response=" + response + "&" + "sync=" + data.sync + "&" + "hash=" + data.hash;
            WebClient().Referer = data.uri.ToString();
            WebClient().Host = data.uri.Host;
            WebClient().ContentType = "application/x-www-form-urlencoded";
            WebClient().UploadString(data.uri, formData);
            string Header = WebClient().ResponseHeaders.Get("Set-Cookie");
            WebClient().Dispose();

            return DownloadManager.WebClient.GetAllCookiesFromHeader(Header, data.uri.Host.ToString());
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
        private static DownloadManager.WebClient WebClient()
        {
            if (_webclient == null) _webclient = new DownloadManager.WebClient();
            if (_cookieCollection != null) _webclient.AddCookie(_cookieCollection);
            return _webclient;
        }
        private static DownloadManager.WebClient WebClient(CookieCollection cookieCollection)
        {
            if (_webclient == null) _webclient = new DownloadManager.WebClient();
            if (cookieCollection != null) _webclient.AddCookie(cookieCollection);
            return _webclient;
        }
    }
}
