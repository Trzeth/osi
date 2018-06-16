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

    public enum IdType
    {
        BeatmapId,
        BeatmapSetId
    }

    public class Beatmapset
    {
        public Server DownloadFrom { get { return _downloadFrom; } }
        public Uri Uri;
        public int BeatmapId;// beatmap_id is per difficulty

        //Major
        public int BeatmapSetId;// beatmapset_id groups difficulties into a set 


        private Server _downloadFrom;
        public Beatmapset(int Id,IdType IdType,Server DownloadFrom)
        {
            switch (IdType)
            {
                case IdType.BeatmapId:
                    BeatmapId = Id;
                    break;
                case IdType.BeatmapSetId:
                    BeatmapSetId = Id;
                    break;
            }
            _downloadFrom = DownloadFrom;
        }
        public Beatmapset(Uri Uri,Server DownloadFrom)
        {
            this.Uri = Uri;
            _downloadFrom = DownloadFrom;
        }
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

    public class DownloadManager
    {
        private BeatmapsetInfo _beatmapsetInfo;

        static List<Beatmapset> _downloadList = new List<Beatmapset>();

        bool _isInsoValid;
        bool _isBloodcatValid;
        

        public DownloadManager()
        {

        }
        public DownloadManager(string Inso_Cookie)
        {
            Inso.Cookie = Inso_Cookie;
            _isInsoValid = true;
        }
        public static void AddToDownloadList(Beatmapset beatmapset)
        {
            if (beatmapset.BeatmapSetId == 0 && beatmapset.BeatmapId == 0) { beatmapset = Prase(beatmapset); }
            _downloadList.Add(beatmapset);
            Check();
        }

        private static Beatmapset Prase(Beatmapset beatmapset)
        {
            UriHepler uri = new UriHepler(beatmapset.Uri);
            if (uri.IsValid)
            {
                switch (uri.Type)
                {
                    case IdType.BeatmapId:
                        beatmapset.BeatmapId = uri.Id;
                        break;
                    case IdType.BeatmapSetId:
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

        private static void Check()
        {
            if(_downloadList.Count > 0)
            {
                for (int i=0; i<_downloadList.Count; i++)
                {
                    string FileName = null;
                    byte[] data = null;


                    var a = new Bloodcat.CAPTCHAData();
                    Bloodcat.GetCAPTCHA(new Uri("http://bloodcat.com/osu/s/791165"), out a);

                    bool succeed;
                    try
                    {
                        switch (_downloadList[i].DownloadFrom)
                        {
                            case Server.Orgin:
                                break;
                            case Server.Inso:
                                data = Inso.Download(_downloadList[i], out FileName);
                                break;
                            case Server.BlooadCat:
                                Bloodcat.GetCAPTCHA(new Uri("http://bloodcat.com/osu/s/791165"),out a);
                                break;
                            case Server.Uugl:
                                data = Uugl.Download(_downloadList[i], out FileName);
                                break;
                        }
                        _FileHelper.FileWrite(data, FileName);
                        succeed = true;
                    }
                    catch (Exception e)
                    {
                        succeed = false;
                        System.Diagnostics.Debug.Write(e.Message);
                    }

                    if (succeed == true)
                    {
                        _downloadList.Remove(_downloadList[i]);
                    }

                }
            }
        }
    }
}