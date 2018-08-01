using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace osi_Desktop.Messages.FirstRun
{
    /// <summary>
    /// Guid.xaml 的交互逻辑
    /// </summary>
    public partial class Guide : Grid
    {
        public new event EventHandler Loaded;


        bool isGetHttpHash;
        bool isGetHttpsHash;
        public static class ExitCode
        {
            public const int Error = -1;//Unhandle Error
            public const int Succeed = 0;
            public const int Alert = 1;
            public const int Failed = 2;
            public const int Continue = 3;
            public const int HandledError = 4;
        }
        private delegate void Register();
        public Guide()
        {
            InitializeComponent();

            Loaded += new EventHandler(delegate 
            {

            });

            Loaded(this,new EventArgs());
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.DataBind, new Register(delegate
            {
                Process LinkMonitor = new Process();
                LinkMonitor.StartInfo.FileName = Environment.CurrentDirectory + @"\LinkMonitor.exe";
                LinkMonitor.StartInfo.Arguments = "--RegisterAsDefaultBrowser FirstRun";
                LinkMonitor.Start();
                LinkMonitor.WaitForExit();

                Process.Start(@"http://config.osi/getHttpHash");
                NamedPipeServerStream server = new NamedPipeServerStream("osi", PipeDirection.In);
                StreamReader sr = new StreamReader(server);
                server.WaitForConnection();
                string link = sr.ReadToEnd();

                if (link == @"http://config.osi/getHttpHash")
                {
                    httpHash_checkBox.IsChecked = true;
                    httpsHash_checkBox.IsChecked = true;
                }
            }));

        }
    }
}
