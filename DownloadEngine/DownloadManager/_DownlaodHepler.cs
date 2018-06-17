using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DownloadEngine.Servers;

namespace DownloadEngine.DownloadManager
{
    internal class _DownlaodHepler
    {
        internal static class FileHelper
        {
            static string _path;
            static FileHelper()
            {
                _path = Environment.CurrentDirectory + @"/download/";
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
    }
}
