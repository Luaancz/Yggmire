using System;
using System.Collections.Generic;

namespace Luaan.Yggmire.SharpClient
{
    class Program
    {
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
            //using (var program = new YggmireGame())
            //    program.Run();

        }
    }
}