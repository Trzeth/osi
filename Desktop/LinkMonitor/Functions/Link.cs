using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinkMonitor.Functions
{
    internal static class LinkFunction
    {
        internal static void SendLink(string link)
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
                osiNotRun();
            }
        }
        private static void osiNotRun()
        {
            if (MessageBox.Show("我们检测到一个问题，这可能是由 osi 运行时出错导致的。"
                               + Environment.NewLine + Environment.NewLine +
                               "您是否希望使用在 osi 中设定的浏览器打开该连接？"
                               , "LinkMonitor", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                MessageBox.Show("Yes");
            }
        }
    }
}
