using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DownloadEngine.Servers
{
    class WebClient:System.Net.WebClient
    {
        public const string UserAgent = "Mozilla / 4.0(compatible;.NET CLR 4.0.30319; osu!in)";
        private CookieContainer _cookieContainer;

        public WebClient()
        {
            Proxy = null;
            Headers.Add(HttpRequestHeader.UserAgent, UserAgent);
        }
        public void AddCookie(Cookie cookie)
        {
            _cookieContainer.Add(cookie);
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = _cookieContainer;
            }
            return request;
        }
    }
}
