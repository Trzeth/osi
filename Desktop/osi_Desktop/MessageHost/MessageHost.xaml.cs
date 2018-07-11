using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using DownloadEngine.Servers;
using DownloadEngine.DownloadManager;
using DownloadEngine;

namespace osi_Desktop.MessageHost
{
    /// <summary>
    /// MessageHost.xaml 的交互逻辑
    /// </summary>
    public partial class MessageHost : Window
    {
        public MessageHost()
        {
            InitializeComponent();

            Loaded += delegate
            {
                Top = System.Windows.Forms.SystemInformation.WorkingArea.Height - Height;
                Left = System.Windows.Forms.SystemInformation.WorkingArea.Width - Width;
            };
        }
        public void Add(Grid page)
        {
            if (Height<= MaxHeight)
            {
                page.Margin = new Thickness(0, 5, 0, 5);
                Host.Children.Add(page);

                if ((Host.Height) <= MaxHeight) 
                {
                    Height = Host.Height;
                }
                else
                {
                    Height = MaxHeight;
                }

                Top = System.Windows.Forms.SystemInformation.WorkingArea.Height - Height;
            }
        }
    }
}
