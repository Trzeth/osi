﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkMonitor.Functions
{
    static class UpdateFunction
    {
        internal static void Update(bool restart)
        {
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
                        Process.Start(Environment.CurrentDirectory + @"\osi.exe");
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
