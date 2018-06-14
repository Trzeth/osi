using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DownloadEngine
{
    internal class _DownlaodHepler
    {
        //internal static class UriHepler2
        //{
        //    public static void Parse(Uri uri,out )
        //    {

        //    }
        //}
        internal class UriHepler
        {
            internal bool IsValid
            {
                get { return _isValid; }
            }
            internal IdType Type
            {
                get { return _idType;}
            }
            internal int Id
            {
                get { return _id; }
            }

            Regex _osuRegex = new Regex(@"^/(?<type>[bsdp])/(beatmap\?b\=)?(?<id>\d+)");
            Regex _osuNewRegex = new Regex(@"^/beatmapsets/(?<sid>\d+)(#(?<mode>\w+)/(?<bid>\d+))?");

            int _id;

            IdType _idType;
            bool _isValid;
            internal UriHepler(Uri uri)
            {
                Parse(uri);
            }
            internal UriHepler(string s)
            {
                try
                {
                    Parse(new Uri(s));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            private void Parse(Uri uri)
            {
                if (uri.Host == "osu.ppy.sh")
                {
                    _isValid = true;

                    Regex osuRegex = new Regex(@"^/(?<type>[bsdp])/(beatmap\?b\=)?(?<id>\d+)");
                    Regex osuNewRegex = new Regex(@"^/beatmapsets/(?<sid>\d+)(#(?<mode>\w+)/(?<bid>\d+))?");

                    if (osuRegex.IsMatch((uri.PathAndQuery)))
                    {
                        Match value = osuRegex.Match(uri.PathAndQuery);
                        switch (value.Groups["type"].Value)
                        {
                            case "b":
                            case "p":
                                _id = int.Parse(value.Groups["id"].Value);
                                _idType = IdType.BeatmapId;
                                break;
                            case "s":
                            case "d":
                                _id = int.Parse(value.Groups["id"].Value);
                                _idType = IdType.BeatmapSetId;
                                break;
                        }
                    }
                    else if (osuNewRegex.IsMatch(uri.PathAndQuery))
                    {
                        Match value = osuNewRegex.Match(uri.PathAndQuery);
                        if (value.Groups["bid"].Value != "")
                        {
                            _id = int.Parse(value.Groups["bid"].Value);
                            _idType = IdType.BeatmapId;
                        }
                        else
                        {
                            _id = int.Parse(value.Groups["sid"].Value);
                            _idType = IdType.BeatmapSetId;
                        }
                    }
                }else if (uri.Host == "bloodcat.com")
                {

                }else if (uri.Host == "inso.link")
                {

                }
                else
                {
                    _isValid = false;
                }

            }
        }
    }
}
