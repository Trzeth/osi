using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DownloadEngine.Servers;
using DownloadEngine.DownloadManager;
using System.Threading;

namespace DownloadEngine.DownloadManager
{
    internal class DownloadHepler
    {
        public class NoServerToChoose : Exception { }
        public class ServerNotAvailable : Exception { }
        public class CookieInvalid : Exception { }
        internal static class FileHelper
        {
            static string _path;
            static FileHelper()
            {
                _path = Environment.CurrentDirectory + @"\download\";
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }
            }
            public static void FileWrite(byte[] file, string fileName)
            {
                File.WriteAllBytes(_path + CheckedFileName(fileName), file);
            }
            private static string CheckedFileName(string fileName)
            {
                string replaceString = " ";
                fileName = fileName.Replace(@"\", replaceString);
                fileName = fileName.Replace(@"/", replaceString);
                fileName = fileName.Replace(@":", replaceString);
                fileName = fileName.Replace(@"*", replaceString);
                fileName = fileName.Replace(@"<", replaceString);
                fileName = fileName.Replace(@">", replaceString);
                fileName = fileName.Replace(@"|", replaceString);

                return fileName;
            }
        }
        internal static Server ChooseServer(BeatmapsetPackage p)
        {
            if (p.FailedServerList.Count == 0)
            {
                if (p.BeatmapsetId >= 700000 && p.BeatmapsetId <= 740000)
                {
                    return Server.Uugl;
                }
                else
                {
                    Random r = new Random();
                    if (r.Next(0, 100) >= 50)
                    {
                        return Server.Inso;
                    }
                    else
                    {
                        return Server.Blooadcat;
                    }
                }
            }
            else
            {
                if (p.FailedServerList.Count < 3)
                {
                    if (p.FailedServerList.Exists(s => s == Server.Uugl) || p.BeatmapsetId <= 700000 || p.BeatmapsetId >= 740000)
                    {
                        if (p.FailedServerList.Exists(s => s == Server.Blooadcat))
                        {
                            return Server.Inso;
                        }
                        else
                        {
                            return Server.Blooadcat;
                        }
                    }
                    else
                    {
                        return Server.Uugl;
                    }
                }
                else
                {
                    throw new NoServerToChoose();
                }

            }
        }
    }
}
