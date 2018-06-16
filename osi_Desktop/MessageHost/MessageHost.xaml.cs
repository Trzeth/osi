using System;
using System.Collections.Generic;
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

            this.Loaded += delegate
            {
                this.Top = System.Windows.Forms.SystemInformation.WorkingArea.Height - this.Height;
                this.Left = System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Width;
            };

            Host.Children.Add(new Message("Purr","Nyao~"));
            Host.Children.Add(new Message("Purrr", "Nyao~~"));
        }
    }
}
