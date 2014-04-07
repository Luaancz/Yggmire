using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luaan.Yggmire.OrleansServerInterfaces.Monitoring;
using Orleans;
using System.Reflection;

namespace Luaan.Yggmire.OrleansServer.Monitoring
{
    public class MonitoringGrain : GrainBase, IMonitoringGrain
    {
        Task<ServerStatusInfo> IMonitoringGrain.GetStatus()
        {
            var status = new ServerStatusInfo();
            status.Revision = typeof(MonitoringGrain).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).OfType<AssemblyInformationalVersionAttribute>().First().InformationalVersion;

            return Task.FromResult(status);
        }
    }
}
