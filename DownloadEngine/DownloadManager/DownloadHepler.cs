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
