using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadEngine.Servers;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using DownloadEngine.DownloadManager;

namespace DownloadEngine
{
    public class Beatmapset
    {
        public class NotValidUri : Exception
        {
            public Uri Uri;

            public NotValidUri(Uri uri) : base("Not a valid Uri")
            {
                this.Data.Add(("Uri"),uri);
            }
        }
        public int BeatmapsetId { get { return _beatmapsetId; } }
        private int _beatmapsetId;
        public List<Beatmap> Beatmap;

        public Beatmapset(int BeatmapsetId)
        {
            _beatmapsetId = BeatmapsetId;
        }
        public Beatmapset(Uri uri)
        {
            _beatmapsetId = ToBeatmapsetId(uri);
        }
        public Beatmapset(string uri)
        {
            _beatmapsetId = ToBeatmapsetId(uri);
            if (_beatmapsetId == -1)
            {
                throw new NotValidUri(new Uri(uri));
            }
        }
        public void Add(Beatmap beatmap)
        {
            Beatmap.Add(beatmap);
        }

        private int ToBeatmapsetId(string uri)
        {
            return ToBeatmapsetId(new Uri(uri));
        }
        private int ToBeatmapsetId(Uri uri)
        {
            int id = -1;

            string Host = uri.Host.ToLower();
            if (Host == "osu.ppy.sh")
            {
                Regex osuRegex = new Regex(@"^/(?<type>[bsdp])/(beatmap\?b\=)?(?<id>\d+)");
                Regex osuNewRegex = new Regex(@"^/beatmapsets/(?<sid>\d+)(#(?<mode>\w+)/(?<bid>\d+))?");

                if (osuRegex.IsMatch((uri.PathAndQuery)))
                {
                    Match value = osuRegex.Match(uri.PathAndQuery);
                    switch (value.Groups["type"].Value)
                    {
                        case "b":
                        case "p":
                            //Beatmap Id
                            id = int.Parse(value.Groups["id"].Value);
                            id = ToBeatmapsetId(id);
                            break;
                        case "s":
                        case "d":
                            //Beatmapset Id
                            id = int.Parse(value.Groups["id"].Value);
                            break;
                    }
                }
                else if (osuNewRegex.IsMatch(uri.PathAndQuery))
                {
                    Match value = osuNewRegex.Match(uri.PathAndQuery);
                    if (value.Groups["sid"].Value != "")
                    //永远为 true Mark
                    {
                        //Beatmapset Id
                        id = int.Parse(value.Groups["sid"].Value);
                    }
                    else
                    {
                        //Beatmap Id
                        id = int.Parse(value.Groups["bid"].Value);
                    }
                }
            }
            else if (Host == "bloodcat.com")
            {

            }
            else if (Host == "inso.link")
            {

            }
            else
            {
                throw new NotValidUri(uri);
            }

            return id;
        }
        private int ToBeatmapsetId(int beatmapId)
        {
            WebClient client = new WebClient();
            client.Method = "Head";
            client.AllowAutoRedirect = false;
            client.DownloadString("http://osu.ppy.sh/b/" + beatmapId);
            return ToBeatmapsetId(new Uri(client.ResponseHeaders["Location"]));
        }
    }
    public class Beatmap
    {
        public int BeatmapId { get { return _beatmapId; } }
        int _beatmapId;

        //public JObject Scores;
        public Beatmap(int beatmapId)
        {
            _beatmapId = beatmapId;
        }
    }
    public enum Status //Rank Status
    {
        Approved,
        Qualified,
        Loved,
        Favourites,
        Pending,
        Graveyard
    }
    public enum Mode
    {
        Standard,
        Taiko,
        Catch_the_Beat,
        Mania
    }
    public enum Genre
    {
        Unspecified,
        Video_Game,
        Anime,
        Rock,
        Pop,
        Other,
        Novelty,
        Hip_Hop,
        Electronic
    }
    public enum Language
    {
        English,
        Chinese,
        French,
        German,
        Italian,
        Japanese,
        Korean,
        Spanish,
        Swedish,
        Instrumental,
        Other
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
        public Status status;
        public string title;
        public string artist;
        public int creatorId;
        public string creator;
        public string tags;
        public string source;
        public Genre genreId;
        public Language languageId;
        public int id;
        public List<BeatmapInfo> BeatmapInfos;
    }
    public class BeatmapInfo
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
}
