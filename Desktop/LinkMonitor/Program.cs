using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinkMonitor.Functions;

namespace LinkMonitor
{
    class Program
    {
        internal const string Update = "--Update";
        internal static class UpdateArgument
        {
            internal const string Restart = "Restart";
        }
        internal const string Ngen = "--Ngen";
        internal const string Link = "--Link";

        internal static int exitCode;
        static void Main(string[] args)
        {
            try
            {
                if (args.Count() > 0)
                {
                    if (args[0] == Link)
                    {
                        LinkFunction.SendLink(args[1]);
                    }
                    else
                    {
                        bool hasSecondArgument = args.Count() > 1 ? true : false;
                        switch (args[0])
                        {
                            case Update:
                                if (hasSecondArgument && args[1] == UpdateArgument.Restart)
                                {
                                    UpdateFunction.Update(true);
                                }
                                else
                                {
                                    UpdateFunction.Update(false);
                                }
                                break;
                            case Ngen:
                                break;
						}
                    }
                }
                else
                {
                    MessageBox.Show("Puuuuuuuu 我才不要理你。"
                                    + Environment.NewLine + Environment.NewLine +
                                    "Ps:如果你想启动 osi 的话 你应该双击 osi.exe");
                }
            }
            catch (Exception e)
            {
#if DEBUG
                MessageBox.Show(e.Message + "   " + e.Source
                                + Environment.NewLine+
                                e.StackTrace);
#endif
                exitCode = ExitCode.Error;
            }
            Environment.Exit(exitCode);
        }
    }
}
