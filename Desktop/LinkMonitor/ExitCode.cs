using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkMonitor
{
    internal static class ExitCode
    {
        internal const int Error = -1;//Unhandle Error
        internal const int Succeed = 0;
        internal const int Alert = 1;
        internal const int Failed = 2;
        internal const int Continue = 3;
        internal const int HandledError = 4;
    }
}
