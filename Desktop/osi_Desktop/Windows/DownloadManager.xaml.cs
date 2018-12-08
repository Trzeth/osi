using DownloadEngine;
using DownloadEngine.DownloadManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace osi_Desktop.Windows
{
    /// <summary>
    /// DownloadManager.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadManager : Window
    {
        private int downloaderCount = 2;
        ObservableCollection<DownloadPackage> packageCollection;
        Queue<string> pendingQueue;
        BackgroundWorker[] downloader;
        BackgroundWorker linkMonitor;
        public DownloadManager()
        {
            InitializeComponent();
            pendingQueue = new Queue<string>();
            packageCollection = new ObservableCollection<DownloadPackage>();
            downloader = new BackgroundWorker[downloaderCount];
            for (int i = 0; i < downloaderCount; i++)
            {
                downloader[i] = new BackgroundWorker();
                downloader[i].DoWork += Downloader_DoWork;
                downloader[i].RunWorkerCompleted += Downloader_Complete;
            }
            linkMonitor = new BackgroundWorker();
            linkMonitor.DoWork += LinkMonitor_DoWork;
            linkMonitor.RunWorkerAsync();
        }

        private void Downloader_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            Process.Start((string)e.Result);
            if (pendingQueue.Count > 0)
            {
                ((BackgroundWorker)sender).RunWorkerAsync(pendingQueue.Dequeue());
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            base.OnClosing(e);
        }

        private void LinkMonitor_DoWork(object sender, DoWorkEventArgs e)
        {
            NamedPipeServerStream server = new NamedPipeServerStream("osi", PipeDirection.In);
            while (true)
            {
                server.WaitForConnection();

                StreamReader sr = new StreamReader(server);
                string link = sr.ReadToEnd();
                server.Disconnect();
                int i;
                for (i = 0; i < downloaderCount; i++)
                {
                    if (!downloader[i].IsBusy)
                    {
                        downloader[i].RunWorkerAsync(link);
                        break;
                    }
                }
                if(i==downloaderCount)pendingQueue.Enqueue(link);
            }
        }
        private void Downloader_DoWork(object sender, DoWorkEventArgs e)
        {
            DownloadEngine.Downloader downloader = new Downloader();
            BeatmapsetPackage beatmapsetPackage = new BeatmapsetPackage(new Beatmapset(((string)e.Argument)));
            downloader.BeatmapsetPackage = beatmapsetPackage;
            DownloadPackage downloadPackage = new DownloadPackage(beatmapsetPackage.Beatmapset);
            beatmapsetPackage.ProgressChanged += delegate(object o, ProgressChangedEventArgs args)
            {
                downloadPackage.ProgressPercentage = args.ProgressPercentage;
             };
            beatmapsetPackage.WriteFileCompleted += delegate(object o, BeatmapsetPackage.WriteFileCompletedArg arg)
            {
                e.Result = arg.Path;
            };
        }
    }
}
