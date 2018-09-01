using DownloadEngine.DownloadManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webclient = DownloadEngine.DownloadManager.WebClient;
using CookieCollection = System.Net.CookieCollection;


namespace DownloadEngine.Servers
{
    public abstract class Server
    {
        protected static CookieCollection _cookieCollection;
        protected static Webclient _webclient;

        internal abstract byte[] Download(BeatmapsetPackage p, out string fileName);
        protected static Webclient WebClient(CookieCollection cookieCollection = null)
        {
            _webclient = new Webclient();
            if (cookieCollection != null) _webclient.AddCookie(cookieCollection);
            return _webclient;
        }
        protected static string BuildQueryString(object key, object value = null,string query = null)
        {
            string s = query;
            if (s == null)
            {
                s += '?';
            }
            else
            {
                s += '&';
            }
            s += key.ToString() + '=' + value;
            return s; 
        }
        protected static string BuildQueryString(Dictionary<object, object> d, string query = null)
        {
            string s = query;
            foreach (KeyValuePair<object,object> k in d)
            {
                s = BuildQueryString(k.Key,k.Value,s);
            }
            return s;
        }
    }
}
