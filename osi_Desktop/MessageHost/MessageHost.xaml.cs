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
using DownloadEngine;

namespace osi_Desktop.MessageHost
{
    /// <summary>
    /// MessageHost.xaml 的交互逻辑
    /// </summary>
    public partial class MessageHost : Window
    {
        DownloadEngine.Servers.Bloodcat.CAPTCHAData CAPTCHAData = new DownloadEngine.Servers.Bloodcat.CAPTCHAData();

        public MessageHost()
        {
            InitializeComponent();

            this.Loaded += delegate
            {
                this.Top = System.Windows.Forms.SystemInformation.WorkingArea.Height - this.Height;
                this.Left = System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Width;
            };

            Host.Children.Add(new Message("Purr","Nyao~"));
            Host.Children.Add(new Message("Purrr", "Nyao~~"));

            BitmapImage imagea = new BitmapImage();

            imagea.BeginInit();
            imagea.StreamSource = new MemoryStream(DownloadEngine.Servers.Bloodcat.GetCAPTCHA(new Uri("http://bloodcat.com/osu/s/317439"), out CAPTCHAData));
            imagea.EndInit();

            image.Source = imagea;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CAPTCHAData.response = textBox.Text;
            DownloadEngine.Servers.Bloodcat._cookieCollection = DownloadEngine.Servers.Bloodcat.PostCAPTCHA(new Uri("http://bloodcat.com/osu/s/317439"),CAPTCHAData);

            DownloadManager.AddToDownloadList(new Beatmapset(new Uri("https://osu.ppy.sh/beatmapsets/628446#osu/1672509"),Server.BlooadCat));

        }
    }
}
