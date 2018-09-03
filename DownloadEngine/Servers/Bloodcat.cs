using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using DownloadEngine.DownloadManager;
using System.Windows.Media.Imaging;
using WebClient = DownloadEngine.DownloadManager.WebClient;

namespace DownloadEngine.Servers
{
    public class Bloodcat:Server
    {
        internal static CookieCollection CookieCollection
        {
            get { return _cookieCollection; }
        }
        private static CookieCollection _cookieCollection;
        public class ReturnInformation
        {
            public class Beatmapset
            {
                public DateTime synced;
                public Status status;
                public string title;
                public string titleU;
                public string artist;
                public string artistU;
                public int creatorId;
                public string creator;
                public DateTime? rankedAt;
                public string tags;
                public string source;
                public Genre genreId;
                public Language languageId;
                public int downloads;
                public int id;
                public Beatmap[] beatmaps;

                public override string ToString()
                {
                    return id + " " + artist + "-" + title;
                }
            }
            public class Beatmap
            {
                public int id;
                public string name;
                public Mode mode;
                public float hp;
                public float cs;
                public float od;
                public float ar;
                public float bpm;
                public int length;
                public float star;
                public string hash_md5;
                public Status status;
                public string author;
            }
            public class Return : List<Beatmapset> { }
        }
        public class QueryArgs
        {
            public class Character
            {
                public const char BeatmapId = 'b';
                public const char BeatmapSetId = 's';
                public const char CreatorId = 'u';
                public const char Other = 'o';
            }
            public enum Status
            {
                Unranked,
                Ranked, 
                Approved, 
                Qualified
            }
            public enum Mode
            {
                Standard,
                Taiko, 
                Catch_the_Beat, 
                Mania
            }
        }
        internal override byte[] Download(BeatmapsetPackage p,out string fileName)
        {
            if(_cookieCollection == null)
            {
                throw new Exception("Bloodcat No Cookie Existed.");
            }
            ReturnInformation.Return r = Search(p.BeatmapsetId.ToString(),QueryArgs.Character.BeatmapSetId);
            ReturnInformation.Beatmapset beatmapset;
            if (r.Count() > 1||r.Count() == 0)
            {
                throw new Exception("Result More than one or Doesn't Exist.");
            }
            else
            {
                beatmapset = r[0];
            }
            BeatmapsetPackage.BeatmapsetInfo info = new BeatmapsetPackage.BeatmapsetInfo();
            info.beatmapsetId = beatmapset.id;
            info.artist = beatmapset.artist;
            info.creator = beatmapset.creator;
            info.title = beatmapset.title;
            p.OnGetInfoCompleted(info);

            byte[] file;
            WebClient client = new WebClient();
            client.AddCookie(_cookieCollection);
            p.OnDownloadProgressChanged(new BeatmapsetPackage.DownloadProgressChangedArgs("Downloading"));
            file = client.DownloadData(Path(p));
            p.OnDownloadProgressChanged(new BeatmapsetPackage.DownloadProgressChangedArgs("Download Complete"));
            fileName = info.beatmapsetId + " " + info.artist + "-" + info.title + ".osz";

            return file;
        }

        #region Set Cookie
        internal static void SetCookie(string cookieString)
        {
            CookieCollection cookieCollection = new CookieCollection();

            Cookie cookie = new Cookie();
            cookie.Name = "obm_human";
            cookie.Path = "/osu/";
            cookie.Domain = "bloodcat.com";
            cookie.Value = cookieString;

            cookieCollection.Add(cookie);
            SetCookie(cookieCollection);
        }
        internal static void SetCookie(CookieCollection cookieCollection)
        {
            _cookieCollection = cookieCollection;
        }
        public static bool IsCookieValid(string cookieString)
        {
            CookieCollection cookieCollection = new CookieCollection();

            Cookie cookie = new Cookie();
            cookie.Name = "obm_human";
            cookie.Path = "/osu/";
            cookie.Domain = "bloodcat.com";
            cookie.Value = cookieString;

            cookieCollection.Add(cookie);
            return IsCookieValid(cookieCollection);
        }
        public static bool IsCookieValid(CookieCollection cookieCollection)
        {
            WebClient client = new WebClient();
            client.AddCookie(cookieCollection);
            client.Method = "Head";
            //Bloodcat 对 Cookie 进行加密 与 User-Agent 配合验证
            try
            {
                client.DownloadData("http://bloodcat.com/osu/s/4079");
                return true;
            }
            catch(WebException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion
        public static ReturnInformation.Return Search(string query,char? character = null,QueryArgs.Status? status = null,QueryArgs.Mode? mode = null,int? page = null)
        {
            WebClient webclient = new WebClient();
            string s = webclient.DownloadString(Path(query,character,status,mode,page));
            ReturnInformation.Return r = JsonConvert.DeserializeObject<ReturnInformation.Return>(s);
            return r;
        }
        #region CAPTCHA
        public class CAPTCHAData
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
                CAPTCHAPage = new WebClient().DownloadString(uri);
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
            WebClient client = new WebClient();
            string formData = "response=" + response + "&" + "sync=" + data.sync + "&" + "hash=" + data.hash;
            client.Referer = data.uri.ToString();
            client.Host = data.uri.Host;
            client.ContentType = "application/x-www-form-urlencoded";
            client.UploadString(data.uri, formData);
            string Header = client.ResponseHeaders.Get("Set-Cookie");
            client.Dispose();

            return WebClient.GetAllCookiesFromHeader(Header, data.uri.Host.ToString());
        }
        #endregion
        private static string Path(BeatmapsetPackage p)
        {
            const string uriRoot = "https://bloodcat.com/osu/";

            return uriRoot + "s/" + p.BeatmapsetId;
        }
        private static string Path(string query, char? character = null, QueryArgs.Status? status = null, QueryArgs.Mode? mode = null, int? page = null)
        {
            string root = "https://bloodcat.com/osu/";

            Dictionary<object, object> d = new Dictionary<object, object>();
            d.Add("mod","json");
            d.Add('q', query);
            if (character != null) { d.Add('c', character); }
            if (status != null) { d.Add('s', status); }
            if (mode != null) { d.Add('m', mode); }
            if (page != null) { d.Add('p', page); }

            return root + BuildQueryString(d);
        }
    }
}
