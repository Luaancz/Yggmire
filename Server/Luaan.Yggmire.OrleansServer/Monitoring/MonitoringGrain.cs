using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luaan.Yggmire.OrleansServerInterfaces.Monitoring;
using Orleans;

namespace Luaan.Yggmire.OrleansServer.Monitoring
{
    public class MonitoringGrain : GrainBase, IMonitoringGrain
    {
        Task<ServerStatusInfo> IMonitoringGrain.GetStatus()
        {
            return Task.FromResult(new ServerStatusInfo());
        }
    }
}
