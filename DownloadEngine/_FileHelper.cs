using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadEngine
{
    class _FileHelper
    {
        static string Directory;
        static _FileHelper()
        {
            Directory = Environment.CurrentDirectory + @"/downlaod/";
        }
        public static void FileWrite(byte[] file,string fileName)
        {
            System.IO.File.WriteAllBytes(Directory + CheckedFileName(fileName), file);
        }
        private static string CheckedFileName(string fileName)
        {
            fileName = fileName.Replace(@"\", "-");
            fileName = fileName.Replace(@"/", "-");
            fileName = fileName.Replace(@":", "-");
            fileName = fileName.Replace(@"*", "-");
            fileName = fileName.Replace(@"<", "-");
            fileName = fileName.Replace(@">", "-");
            fileName = fileName.Replace(@"|", "-");

            return fileName;
        }
    }
}
