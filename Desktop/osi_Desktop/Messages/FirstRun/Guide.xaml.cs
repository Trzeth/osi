using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        Process LinkMonitor;
        public static class ExitCode
        {
            public const int Error = -1;//Unhandle Error
            public const int Succeed = 0;
            public const int Alert = 1;
            public const int Failed = 2;
            public const int Continue = 3;
            public const int HandledError = 4;
        }

        public Guide()
        {
            InitializeComponent();
            LinkMonitor = new Process();
            LinkMonitor.StartInfo.FileName = Environment.CurrentDirectory + @"\LinkMonitor.exe";
            LinkMonitor.StartInfo.Arguments = "--RegisterAsDefaultBrowser FirstRun";
            LinkMonitor.Start();
            LinkMonitor.WaitForExit();
            if(LinkMonitor.ExitCode == ExitCode.Succeed)
            {

            }
            else if (LinkMonitor.ExitCode == ExitCode.Continue)
            {
                Process.Start(@"http://config.osi/getHttpHash");

            }
            else
            {

            }
        }
    }
}
