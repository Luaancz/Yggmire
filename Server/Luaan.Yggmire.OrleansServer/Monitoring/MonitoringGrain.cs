using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luaan.Yggmire.OrleansServerInterfaces.Monitoring;
using Orleans;
using System.Reflection;
using Luaan.Yggmire.OrleansInterfaces;
using Orleans.Providers;

namespace Luaan.Yggmire.OrleansServer.Monitoring
{
    [StorageProvider(ProviderName="MemoryStore")]
    public class MonitoringGrain : Grain<IMonitoringStatus>, IMonitoringGrain
    {
        Task<ServerStatusInfo> IMonitoringGrain.GetStatus()
        {
            var status = new ServerStatusInfo();
            status.Revision = typeof(MonitoringGrain).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute)).OfType<AssemblyInformationalVersionAttribute>().First().InformationalVersion;
            status.PlayerCount = State.PlayerCount;

            return Task.FromResult(status);
        }

        Task IMonitoringGrain.RegisterSession()
        {
            State.PlayerCount++;

            return State.WriteStateAsync();
        }

        Task IMonitoringGrain.UnregisterSession()
        {
            State.PlayerCount--;

            return State.WriteStateAsync();
        }
    }

    public interface IMonitoringStatus : IGrainState
    {
        int PlayerCount { get; set; }
    }
}
