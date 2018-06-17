using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadEngine.Servers;
using static DownloadEngine.DownloadManager._DownlaodHepler;

namespace DownloadEngine.DownloadManager
{
    internal class BeatmapsetPackage
    {
        internal Server Server { get { return _server; } }
        internal int BeatmapsetId { get { return _beatmapsetId; } }

        Server _server;
        int _beatmapsetId;
        internal BeatmapsetPackage(Beatmapset beatmapset, Server server)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
            _server = server;
        }
        internal BeatmapsetPackage(Beatmapset beatmapset)
        {
            _beatmapsetId = beatmapset.BeatmapsetId;
        }
    }
    internal class PendingBeatmapset : BeatmapsetPackage
    {
        public PendingBeatmapset(Beatmapset beatmapset, Server server) : base(beatmapset, server) { }
        public PendingBeatmapset(Beatmapset beatmapset) : base(beatmapset) { }
    }
    class DownloadManager
    {
        static List<BeatmapsetPackage> _pendingList = new List<BeatmapsetPackage>();

        bool _isInsoValid;
        bool _isBloodcatValid;

        public DownloadManager()
        {

        }
        public DownloadManager(string InsoCookie)
        {
            Inso.SetCookie(InsoCookie);
            _isInsoValid = true;
        }
        public static void Download(Beatmapset beatmapset,Server server)
        {
            BeatmapsetPackage beatmapsetPackage = new BeatmapsetPackage(beatmapset,server);
            _pendingList.Add(beatmapsetPackage);
            Check();
        }
        public static void Download(Beatmapset beatmapset)
        {
            BeatmapsetPackage beatmapsetPackage = new BeatmapsetPackage(beatmapset);
            _pendingList.Add(beatmapsetPackage);
            Check();
        }
        private static void Check()
        {
            if (_pendingList.Count > 0)
            {
                for (int i = 0; i < _pendingList.Count; i++)
                {
                    string fileName = null;
                    byte[] data = null;

                    bool succeed;
                    try
                    {
                        switch (_pendingList[i].Server)
                        {
                            case Server.Orgin:
                                break;
                            case Server.Inso:
                                data = Inso.Download(_pendingList[i], out fileName);
                                break;
                            case Server.BlooadCat:
                                data = Bloodcat.Downlaod(_pendingList[i], out fileName);
                                break;
                            case Server.Uugl:
                                data = Uugl.Download(_pendingList[i], out fileName);
                                break;
                        }
                        FileHelper.FileWrite(data, fileName);
                        succeed = true;
                    }
                    catch (Exception e)
                    {
                        succeed = false;
                        System.Diagnostics.Debug.Write(e.Message);
                    }

                    if (succeed == true)
                    {
                        _pendingList.Remove(_pendingList[i]);
                    }

                }
            }
        }

    }
}
