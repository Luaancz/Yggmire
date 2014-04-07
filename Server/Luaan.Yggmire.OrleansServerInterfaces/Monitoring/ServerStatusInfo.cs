using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace Luaan.Yggmire.OrleansServerInterfaces.Monitoring
{
    [Immutable]
    public class ServerStatusInfo
    {
        public string Revision { get; set; }
        public int? PlayerCount { get; set; }
    }
}
