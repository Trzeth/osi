using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace osi_Desktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        static MessageHost.MessageHost MessageHost;
        App()
        {
            MessageHost = new MessageHost.MessageHost();
            MessageHost.Show();

            MessageHost.Add(new Messages.FirstRun.Guide());
        }
    }
}
