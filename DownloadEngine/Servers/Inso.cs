using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;
using DownloadEngine.DownloadManager;

namespace DownloadEngine.Servers
{
    static class Inso
    {
        public static float current_timestamp;
        private static System.Net.CookieCollection _cookieCollection;//do_not_remove_this_0w0
        public class User
        {
            public string username;
            public int user_id;
            public int cd_timestamp;
            public int vip_level;
            //"username":"Trzeth","user_id":"5106629","cd_timestamp":0,"vip_level":"2","current_timestamp":1512263695.486
        }
        private enum Methods
        {
            user,
            download,
            ci_token,
        }
        private class Packages
        {
            public Uri game_play
            {
                get { return uri[0]; }
                set { uri[0] = value; }
            }
            public Uri skin_sound
            {
                get { return uri[1]; }
                set { uri[1] = value; }
            }
            public Uri skin_image
            {
                get { return uri[2]; }
                set { uri[2] = value; }
            }
            public Uri video
            {
                get { return uri[3]; }
                set { uri[3] = value; }
            }
            public Uri storyboard
            {
                get { return uri[4]; }
                set { uri[4] = value; }
            }
            public Uri central
            {
                get { return uri[5]; }
                set { uri[5] = value; }
            }
            public Uri[]  uri = new Uri[6];
            // game_play skin_sound skin_image video storyboard central
        }
        private static string ApiPath(Methods methods,string valve)
        {
            string apiRoot = "http://inso.link/api/";
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
        internal static byte[] Download(BeatmapsetPackage p,out string fileName)
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
            while (!finished)
            {

                result = JObject.Parse(WebClient().DownloadString(ApiPath(Methods.download, bValve)));

                switch ((int)result["return"])
                {
                    case -1:
                    case 5:
                    case 999:
                    case 1100: finished = true; break;
                    
                    case 100: finished = true; data = Download(result); break;
                    case 200: case 300: case 400: case 500:
                    case 201: case 301: case 401: case 501:
                    case 202: case 302: case 402: case 502:
                    case 203: case 303: case 403: case 503: Thread.Sleep(2000); break;
                }
            }

            fileName = result["mapset"] + " " + result["info"]["artist"] + " - " + result["info"]["title"] + @".osz";

            return data;
        }
        private static byte[] Download(JObject result)
        {
            Packages packages = new Packages();
            JToken package_url = result["package_url"];

            packages.game_play = (Uri)package_url["game_play"];
            packages.skin_sound = ((string)package_url["skin_sound"] != string.Empty ? (Uri)package_url["skin_sound"] : null);
            packages.skin_image = ((string)package_url["skin_image"] != string.Empty ? (Uri)package_url["skin_image"] : null);
            packages.video = ((string)package_url["video"] != string.Empty ? (Uri)package_url["video"] : null);
            packages.storyboard = ((string)package_url["storyboard"] != string.Empty ? (Uri)package_url["storyboard"] : null);
            packages.central = (Uri)package_url["central"];

            JObject centralIndexR = JObject.Parse(WebClient().DownloadString(ApiPath(Methods.ci_token,(string)result["ci_token"])));

            int combination = 0;
            byte[] file = new byte[0];
            for (int i = 0;i <= 5;i++)
            {
                if(packages.uri[i] != null)
                {
                    byte[] data = WebClient().DownloadData(packages.uri[i].AbsoluteUri);
                    byte[] newData = Decode(data, centralIndexR["central_index"]);
                    Console.WriteLine("{0} {1}",data.Length,newData.Length);
                    if (i != 5)
                    {

                        combination += (int)Math.Pow(2, i);

                        file = file.Concat(newData).ToArray();
                    }
                    else
                    {
                        //Are you fu**ing kiding me?!! Repeat?!!

                        int start = (int)centralIndexR["central_index"][combination.ToString()][0];
                        int end = (int)centralIndexR["central_index"][combination.ToString()][1];

                        byte[] b = newData.Skip(start).Take(end).ToArray();

                        file = file.Concat(newData.Skip(start).Take(end)).ToArray();
                    }
                }
            }
            GC.Collect();

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
        private static byte[] Decode(byte[] data, JToken central_index)
        {
            JValue[] centralIndex = central_index["0"].Cast<JValue>().ToArray();

            byte[] newData = new byte[data.Length];

            if (data.Length >= 64)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    if (j < centralIndex.Count())
                    {
                        newData[j] = (byte)(int.Parse(centralIndex[j].ToString()) ^ data[j]);
                    }
                    else
                    {
                        newData[j] = (data[j]);
                    }
                }
            }

            centralIndex = null;
            data = null;
            central_index = null;

            return newData;
        }
        private static WebClient WebClient()
        {
            WebClient webclient = new WebClient();
            webclient.AddCookie(_cookieCollection);
            return webclient;
        }
        internal static void SetCookie(string cookieString)
        {
            if(_cookieCollection == null)
            {
                _cookieCollection = new System.Net.CookieCollection();
            }

            var cookie = new System.Net.Cookie();
            cookie.Name = "do_not_remove_this_0w0";
            cookie.Path = "/";
            cookie.Domain = ".inso.link";
            cookie.Value = cookieString;

            _cookieCollection.Add(cookie);
        }
    }
}
