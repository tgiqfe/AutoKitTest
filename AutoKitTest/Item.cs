using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKitTest
{
    internal class Item
    {
        public static readonly string WorkDir =
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

    }
}
