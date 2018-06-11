using System;
using static DownloadEngine._DownlaodHepler;
using DownloadEngine.Servers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DownloadEngine
{
    public enum Server
    {
        Orgin,
        Inso,
        BlooadCat,
        Uugl
    }

    public class Beatmapset
    {
        public Server DownloadFrom = Server.Inso;
        public Uri Uri;
        public int BeatmapId;// beatmap_id is per difficulty

        //Major
        public int BeatmapSetId;// beatmapset_id groups difficulties into a set 
        
        public int Size;
    }

    public class BeatmapsetInfo
    {
        /*"beatmaps": "472863,Beginner,1.763,0;472916,Normal,2.254,0;698106,Advanced,3.042,0;472797,Low's Hyper,3.623,0;698708,Another,4.697,0;481813,Priti's Black Another,5.091,0;474115,Elvis' Extra,5.632,0;472619,Extreme,5.686,0", 
        "creator": "P o M u T a", 
        "title": "Bad Maniacs", 
        "bpm": 195, 
        "drain_time": 118, 
        "artist": "kors k as teranoid", 
        "approved": 1, 
        "tags": "reflec beat limelight bemani plus rb konami pomuta low jacob industrial ways for liberation evilelvis priti road fighters 17 sirius groovin'!!"
        */
        public string creator;
        public string title;
        public int bpm;
        public int drain_time;
        public string artist;
        public int approved;
    }

    public static class Count
    {
        public static class DownloadCount
        {
            public static int Orgin;
            public static int Inso;
            public static int BlooadCat;
            public static int Uugl;
        }
        public static int[] transCount = new int[4];
    }
    public class DownloadMgr
    {
        public BeatmapsetInfo _beatmapsetInfo;

        static List<Beatmapset> _downloadList;

        bool _isInsoValid;
        bool _isBloodcatValid;
        

        public void DownloaderMgr()
        {
            if (Inso.Cookie == null) { }

        }
        public void DownloaderMgr(string Inso_Cookie)
        {
            Inso.Cookie = Inso_Cookie;
            _isInsoValid = true;
        }
        public static void AddToDownloadList(Beatmapset beatmapset)
        {
            if (beatmapset.BeatmapSetId == 0 || beatmapset.BeatmapId == 0) { beatmapset = Prase(beatmapset); }
            _downloadList.Add(beatmapset);
        }

        private static Beatmapset Prase(Beatmapset beatmapset)
        {
            UriHepler uri = new UriHepler(beatmapset.Uri);
            if (uri.IsValid)
            {
                switch (uri.Type)
                {
                    case UriHepler.IdType.BeatmapId:
                        beatmapset.BeatmapId = uri.Id;
                        break;
                    case UriHepler.IdType.BeatmapSetId:
                        beatmapset.BeatmapSetId = uri.Id;
                        break;
                }
            }
            else
            {
                throw new Exception("Not A Valid URI.");
            }
            return beatmapset;
        }

        //private void Download()
        //{
        //    switch (_beatmapset.DownloadFrom)
        //    {
        //        case Server.Orgin:
        //            break;
        //        case Server.Inso:
        //            Servers.Inso inso = new Servers.Inso();
        //            inso.Download(_beatmapset);
        //            break;
        //        case Server.BlooadCat:
        //            break;
        //        case Server.Uugl:
        //            break;
        //    }
        //}
    }
}