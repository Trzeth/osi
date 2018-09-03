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
    public class DownloadHepler
    {
        public class NoServerToChoose : Exception { }
        public class ServerNotAvailable : Exception { }
        public class CookieInvalid : Exception { }
        public static class FileHelper
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
            public static string FileWrite(byte[] file, string fileName)
            {
                string path = _path + CheckedFileName(fileName);
                File.WriteAllBytes(path, file);
                return path;
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
            Server[] order = new Server[] {Server.Blooadcat,Server.Inso,Server.Orgin };
            Server server = new Server();

            for(int i = 0;i < 3;i++)
            {
                Server s = order[i];
                if(!p.FailedServerList.Exists(x => x == s))
                {
                    server = s;
                    break;
                }
                else if(i == 2)
                {
                    throw new Exception();
                }
            }
            return server;
        }
    }
}
