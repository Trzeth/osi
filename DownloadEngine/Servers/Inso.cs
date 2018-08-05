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
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace DownloadEngine.Servers
{
    class Inso:Server
    {
        public static float current_timestamp;
        static System.Net.CookieCollection _cookieCollection;//do_not_remove_this_0w0
        static WebClient _webclient;
        public class User
        {
            public string username;
            public int user_id;
            public int cd_timestamp;
            public int vip_level;
            //"username":"Trzeth","user_id":"5106629","cd_timestamp":0,"vip_level":"2","current_timestamp":1512263695.486
        }
        enum Methods
        {
            user,
            download,
            ci_token,
        }
        static string ApiPath(Methods methods,string valve)
        {
            //string apiRoot = "http://inso.link/api/";
            string apiRoot = "http://127.0.0.1:8080/";
            string path;
            switch (methods)
            {
                case Methods.user:
                    path = apiRoot + "i.php?";
                    break;
                case Methods.download:
                    path = apiRoot + "n.php?" + valve + "&ir=1" + '&';
                    //valve: [m/b]=[id]
                    break;
                case Methods.ci_token:
                    path = apiRoot + "s.php?" + "ci_token=" + valve + '&';
                    break;
                default:
                    path = apiRoot;
                    break;
            }
            path = path + "source=osi";
            return path;
        }
        public class ReturnInformation
        {
            public class Base
            {
                //一定要全部都设置为 public
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
                public Dictionary<string, string> info;
                public int package_available;
                public int cd_mapset;
                public string expire_timestamp;
                public Dictionary<string, Uri> package_url;
                public int mapset;
                public float cd_ratio;
                public string error;
                public string ci_token;
            }
            public class CentralIndex : Base
            {
                public CentralIndex() : base() { }
                public Dictionary<int, int[]> central_index;
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

            string bValve;
            byte[] data = null;

            bValve = "m" + "=" + p.BeatmapsetId;

            bool finished = false;
            JObject result = null;
            ReturnInformation.Download d = null;
            while (!finished)
            {
                var s = WebClient().DownloadString(ApiPath(Methods.download, bValve));
                d = JsonConvert.DeserializeObject<ReturnInformation.Download>(s);

                switch (d.returnCode)
                {
                    case -1:
                    case 5:
                    case 999:
                    case 1100: finished = true; break;
                    
                    case 100: finished = true; data = Download(d.package_url,d.ci_token); break;
                    case 200: case 300: case 400: case 500:
                    case 201: case 301: case 401: case 501:
                    case 202: case 302: case 402: case 502:
                    case 203: case 303: case 403: case 503: Thread.Sleep(2000); break;
                }
            }

            fileName = d.mapset + " " + d.info["artist"] + "-" + d.info["title"] + ".osz";

            return data;
        }
        private static byte[] Download(Dictionary<string,Uri> uris,string ci_token)
        {
            var s = WebClient().DownloadString(ApiPath(Methods.ci_token, ci_token));
            ReturnInformation.CentralIndex c = JsonConvert.DeserializeObject<ReturnInformation.CentralIndex>(s);

            int combination = 0;
            byte[] file = new byte[0];
            for (int i = 0;i <= 5;i++)
            {
                Uri uri = uris.Values.ToArray()[i];
                if(uri != null)
                {
                    byte[] data = WebClient().DownloadData(uri.AbsoluteUri);
                    byte[] newData = Decode(data, c.central_index[0]);
                    if (i != 5)
                    {
                        combination += (int)Math.Pow(2, i);

                        file = file.Concat(newData).ToArray();
                    }
                    else
                    {
                        //Are you fu**ing kiding me?!! Repeat?!!

                        int start = c.central_index[combination][0];
                        int end = c.central_index[combination][1];

                        byte[] b = newData.Skip(start).Take(end).ToArray();

                        file = file.Concat(newData.Skip(start).Take(end)).ToArray();
                    }
                }
            }
            return file;
        }
        internal static User GetUserStatus()
        {
            User user = new User();
            JObject result = JObject.Parse(WebClient().DownloadString(ApiPath(Methods.user,null)));

            user.username = (string)result["username"];
            user.user_id = (int)result["user_id"];
            user.cd_timestamp = (int)result["cd_timestamp"];
            user.vip_level = (int)result["vip_level"];
            current_timestamp = (float)result["current_timestamp"];

            return user;
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
        private static WebClient WebClient()
        {
            if (_webclient == null) _webclient = new WebClient();
            if(_cookieCollection != null) _webclient.AddCookie(_cookieCollection);
            return _webclient;
        }
        private static WebClient WebClient(System.Net.CookieCollection cookieCollection)
        {
            if (_webclient == null) _webclient = new WebClient();
            if (cookieCollection != null) _webclient.AddCookie(cookieCollection);
            return _webclient;
        }
        internal static bool SetCookie(string cookieString)
        {

            var cookieCollection = new System.Net.CookieCollection();

            var cookie = new System.Net.Cookie();
            cookie.Name = "do_not_remove_this_0w0";
            cookie.Path = "/";
            cookie.Domain = ".inso.link";
            cookie.Value = cookieString;

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
        internal static bool SetCookie(System.Net.CookieCollection cookieCollection)
        {
            if (IsCookieValid(cookieCollection))
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
        public static bool IsCookieValid(System.Net.CookieCollection cookieCollection)
        {
            JObject result = JObject.Parse(WebClient(cookieCollection).DownloadString("http://inso.link/api/i.php"));
            if ((string)result["logged_in"] == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
