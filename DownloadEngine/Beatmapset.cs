using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadEngine.Servers;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace DownloadEngine
{
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
    public class Beatmapset
    {
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
            _beatmapsetId = ToBeatmapsetId(new Uri(uri));
        }
        public void Add(Beatmap beatmap)
        {
            Beatmap.Add(beatmap);
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
                throw new Exception();
            }

            return id;
        }
        private int ToBeatmapsetId(int BeatmapId)
        {
            return -1;
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
}
