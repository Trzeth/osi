using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;
using DownloadEngine.DownloadManager;
using WebClient = DownloadEngine.DownloadManager.WebClient;
using CookieCollection = System.Net.CookieCollection;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace DownloadEngine.Servers
{
    public class Inso:Server
    {
        internal static CookieCollection CookieCollection
        {
            get { return _cookieCollection; }
        }
        private static CookieCollection _cookieCollection;
        private BeatmapsetPackage package;
        //Cookie do_not_remove_this_0w0
        enum Methods
        {
            user,
            download,
            ci_token,
        }
        public enum Package
        {
            game_play,
            skin_sound,
            skin_image,
            video,
            storyboard,
            central
        }
        private class QueryArgs
        {
            public class Download
            {
                public const string BeatmapsetId = "m";
                public const string BeatmapId = "b";
            }
            public const string Ci_Token = "ci_token";
        }
        private static string Path(Methods methods,string Key = null,string Value = null)
        {
            string root = "http://inso.link/api/";
            Dictionary<object, object> d = new Dictionary<object, object>();
            //string apiRoot = "http://127.0.0.1:8080/";
            string path;
            switch (methods)
            {
                case Methods.user:
                    path = root + "i.php";
                    break;
                case Methods.download:
                    path = root + "n.php";
                    d.Add("ir","1");
                    //valve: [m/b]=[id]
                    break;
                case Methods.ci_token:
                    path = root + "s.php";
                    break;
                default:
                    path = root;
                    break;
            }
            if (Key != null && Value != null)
            {
                d.Add(Key, Value);
            }
            d.Add("source","osi");
            return path + BuildQueryString(d);
        }
        public class ReturnInformation
        {
            public class Base
            {
                //一定要全部都设置为 public
                public string error;
                public int returnCode;
                public int cd_timestamp;
                public string current_timestamp;

                [JsonExtensionData]
                private IDictionary<string, JToken> _data;
                public Base()
                {
                    _data = new Dictionary<string, JToken>();
                }
                [OnDeserialized]
                private void OnDeserialized(StreamingContext context)
                {
                    returnCode = (int)_data["return"];
                }
            }
            public class Download:Base
            {
                public Download() : base() { }
                public Infomation info;
                public int package_available;
                public int cd_mapset;
                public string expire_timestamp;
                public Dictionary<string, Uri> package_url;
                public int mapset;
                public float cd_ratio;
                public string ci_token;
            }
            public class CentralIndex : Base
            {
                public CentralIndex() : base() { }
                public Dictionary<int, int[]> central_index;
            }
            public class User:Base
            {
                public User() : base() { }
                public bool logged_in;
                public string username;
                public string user_id;
                public int vip_level;
            }
            public class Infomation
            {
                [JsonExtensionData]
                private IDictionary<string, JToken> _data;
                public Infomation()
                {
                    _data = new Dictionary<string, JToken>();
                }
                [OnDeserialized]
                private void OnDeserialized(StreamingContext context)
                {
                    string[] sArray = _data["beatmaps"].ToString().Split(';');
                    beatmapCollection = new Beatmap[sArray.Count()];
                    int i = 0;
                    foreach (string s in sArray)
                    {
                        string[] t = s.Split(',');
                        Beatmap beatmap = new Beatmap();
                        beatmap.id = int.Parse(t[0]);
                        beatmap.name = t[1];
                        beatmap.star = float.Parse(t[2]);
                        beatmap.mode = (Mode)int.Parse(t[3]);
                        beatmapCollection[i] = beatmap;
                    }
                }

                public Beatmap[] beatmapCollection;
                public string creator;
                public string title;
                public float bpm;
                public int drain_time;
                public string artist;
                public int approved;
                public string tags;
            }
            public class Beatmap
            {
                public int id;
                public string name;
                public Mode mode;
                public float star;
            }
        }
        internal override byte[] Download(BeatmapsetPackage p,out string fileName)
        {
            /*
            "package_url": {
                "storyboard": "", 
                "central": "http://7xkvc9.com1.z0.glb.clouddn.com/199244/central.inso?e=1512266906&token=yyRfcFuGuTEVPCMRl1TvF_GD3zx6nBalXoZKwupW:csvHoX94Lkd6QPLo0CY8f74tybg=", 
                "skin_image": "", 
                "game_play": "http://7xkvc9.com1.z0.glb.clouddn.com/199244/game_play.inso?e=1512266906&token=yyRfcFuGuTEVPCMRl1TvF_GD3zx6nBalXoZKwupW:JXaJsmAKkV0Bn23kgk2b5iDf-hM=", 
                "video": "", 
                "skin_sound": "http://7xkvc9.com1.z0.glb.clouddn.com/199244/skin_sound.inso?e=1512266906&token=yyRfcFuGuTEVPCMRl1TvF_GD3zx6nBalXoZKwupW:JFkYwLruGbxO1rDeE5NyAhOn05E="
            },       
            */
            package = p;
            byte[] data = null;

            bool finished = false;
            ReturnInformation.Download d = null;
            while (!finished)
            {
                var s = WebClient().DownloadString(Path(Methods.download, QueryArgs.Download.BeatmapsetId,p.BeatmapsetId.ToString()));
                d = JsonConvert.DeserializeObject<ReturnInformation.Download>(s);

                var info = new BeatmapsetPackage.BeatmapsetInfo();
                info.artist = d.info.artist;
                info.beatmapsetId = d.mapset;
                info.creator = d.info.creator;
                info.title = d.info.title;
                package.OnGetInfoCompleted(info);

                switch (d.returnCode)
                {
                    case -1:
                    case 5:
                    case 999:
                    case 1100: throw new Exception(); break;
                    
                    case 100: finished = true; data = Download(d.package_url,d.ci_token); break;
                    case 200: case 300: case 400: case 500:
                    case 201: case 301: case 401: case 501:
                    case 202: case 302: case 402: case 502:
                    case 203: case 303: case 403: case 503: Thread.Sleep(2000); break;
                }
            }

            fileName = d.mapset + " " + d.info.artist + "-" + d.info.title + ".osz";

            return data;
        }
        private byte[] Download(Dictionary<string,Uri> uris,string ci_token,List<Package> selectedPackages = null)
        {
            string[] order = SortedPackage(selectedPackages);
            var s = WebClient().DownloadString(Path(Methods.ci_token, QueryArgs.Ci_Token,ci_token));
            ReturnInformation.CentralIndex c = JsonConvert.DeserializeObject<ReturnInformation.CentralIndex>(s);

            int combination = 0;
            byte[] file = new byte[0];

            int i = 0;
            foreach (string packageName in order)
            {
                if (uris[packageName] != null)
                {
                    byte[] data = WebClient().DownloadData(uris[packageName].AbsoluteUri);
                    byte[] newData = Decode(data, c.central_index[0]);

                    if (packageName != "central")
                    {
                        combination += (int)Math.Pow(2, i);
                        file = file.Concat(newData).ToArray();
                    }
                    else
                    {
                        //Are you fu**ing kiding me?!! Repeat?!!
                        int start = c.central_index[combination][0];
                        int end = c.central_index[combination][1];

                        file = file.Concat(newData.Skip(start).Take(end)).ToArray();
                    }
                    package.OnDownloadProgressChanged(new BeatmapsetPackage.DownloadProgressChangedArgs("Download Package " + packageName + " Completed"));
                }
                //怕 是 有 BUG
                i++;
            }
            package.OnDownloadProgressChanged(new BeatmapsetPackage.DownloadProgressChangedArgs("Download Completed"));
            return file;
        }
        private static byte[] Decode(byte[] data, int[] central_index)
        {
            byte[] newData = new byte[data.Length];

            if (data.Length >= 64)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    if (j < central_index.Count())
                    {
                        newData[j] = (byte)(int.Parse(central_index[j].ToString()) ^ data[j]);
                    }
                    else
                    {
                        newData[j] = (data[j]);
                    }
                }
            }

            data = null;
            central_index = null;

            return newData;
        }
        internal static void SetCookie(string cookieString)
        {
            var cookieCollection = new CookieCollection();

            var cookie = new System.Net.Cookie();
            cookie.Name = "do_not_remove_this_0w0";
            cookie.Path = "/";
            cookie.Domain = ".inso.link";
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
            var cookieCollection = new CookieCollection();

            var cookie = new System.Net.Cookie();
            cookie.Name = "do_not_remove_this_0w0";
            cookie.Path = "/";
            cookie.Domain = ".inso.link";
            cookie.Value = cookieString;

            cookieCollection.Add(cookie);
            return IsCookieValid(cookieCollection);
        }
        public static bool IsCookieValid(CookieCollection cookieCollection)
        {
            WebClient client = new WebClient();
            client.AddCookie(cookieCollection);
            string s = client.DownloadString(Path(Methods.user));
            ReturnInformation.User user = JsonConvert.DeserializeObject<ReturnInformation.User>(s);
            if (user.logged_in)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string[] SortedPackage(List<Package> selectedPackages)
        {
            string[] defaultOrder = new string[]{ "game_play", "skin_sound","skin_image","video","storyboard","central"};
            if (selectedPackages != null)
            {
                string[] sortedPackages = new string[selectedPackages.Count];
                int i = 0;
                foreach (string s in defaultOrder)
                {
                    if (selectedPackages.Exists(x => nameof(x) == s))
                    {
                        sortedPackages[i] = s;
                        i++;
                    } 
                }
                return sortedPackages;
            }
            else
            {
                return defaultOrder;
            }
        }
        protected WebClient WebClient()
        {
            WebClient client = new WebClient();
            client.AddCookie(_cookieCollection);
            return client;
        }
    }
}
