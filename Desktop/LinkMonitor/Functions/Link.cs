using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinkMonitor.Helper;
using System.Windows.Forms;

namespace LinkMonitor.Functions
{
    internal static class LinkFunction
    {
        internal static void SendLink(string link)
        {
			ConfigModel configModel;
			ConfigHelper configHelper = new ConfigHelper();
			if (configHelper.ReadConfigFromFile())
			{
				configModel = configHelper.ConfigModel;
			}
			else
			{
				configModel = new ConfigModel();
				configModel.IsRunning = false;
			}

			if (configModel.IsRunning)
			{
				NamedPipeClientStream client = new NamedPipeClientStream(".", "osi", PipeDirection.Out);
				try
				{
					while (true)
					{
						try
						{
							client.Connect(1000);
							break;
						}
						catch (IOException)
						{
							Thread.Sleep(500);
						}
					}
					StreamWriter sw = new StreamWriter(client);
					sw.Write(link);
					sw.Flush();

					sw.Dispose();
					client.Dispose();
				}
				catch (TimeoutException e)
				{
					if (NotRunDailog(link))
					{
						configModel.Registry.UserBrowserRegistry.OpenUrl(new Uri(link));
					}
					configHelper.ChangeRunningStatus(false);
					configHelper.SaveConfig();
				}
			}
			else
			{
				if(configModel.OSVersion == OSVersion.Windows_10_Above_And_Include_Build10122)
				{
					configModel.Registry.UserBrowserRegistry.OpenUrl(new Uri(link));
				}
				else
				{
					if (NotRunDailog(link))
					{
						configModel.Registry.UserBrowserRegistry.OpenUrl(new Uri(link));
					}
				}
			}

		}
        private static bool NotRunDailog(string link)
        {
			return (MessageBox.Show("我们检测到一个问题，这可能是由 osi 运行时出错导致的。"
								+ Environment.NewLine +
							    "我们接受到一个链接，但是由于相关配置这是不可能的。"
								+ Environment.NewLine +
								"您是否希望使用在 osi 中设定的浏览器打开该连接？"
								+ Environment.NewLine +
								link
							   , "osi LinkMonitor", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes);
        }
    }
}
