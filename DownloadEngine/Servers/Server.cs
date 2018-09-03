﻿using DownloadEngine.DownloadManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient = DownloadEngine.DownloadManager.WebClient;
using CookieCollection = System.Net.CookieCollection;

namespace DownloadEngine.Servers
{
    public abstract class Server
    {
        internal abstract byte[] Download(BeatmapsetPackage p, out string fileName);
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
                if (k.Key != null && k.Value != null)
                {
                    s = BuildQueryString(k.Key, k.Value, s);
                }
            }
            return s;
        }
    }
}
