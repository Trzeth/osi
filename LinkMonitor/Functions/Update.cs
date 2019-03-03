using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinkMonitor.Functions
{
    static class UpdateFunction
    {
        internal static void Update(bool restart)
        {
			MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory + @"osi.Desktop.exe");
			UpdateFile();
            if (restart)
            {
                while (true)
                {
                    if (Process.GetProcessesByName("osi").Count() > 0)
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"osi.Desktop.exe");
                        break;
                    }
                }
            }
        }
        private static void UpdateFile()
        {
            string[] newFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.new");
            foreach (string newFile in newFiles)
            {
                string currentFile = newFile.Remove(newFile.LastIndexOf('.'), 4);
                if (File.Exists(currentFile))
                {
                    string oldFile = currentFile + ".old";
                    if (File.Exists(oldFile)) File.Delete(oldFile);
                    File.Move(currentFile, oldFile);
                }
                File.Move(newFile, currentFile);
            }
        }
    }
}
