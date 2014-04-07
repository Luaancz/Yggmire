using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace Luaan.Yggmire.OrleansServerInterfaces.Monitoring
{
    public interface IMonitoringGrain : IGrain
    {
        Task<ServerStatusInfo> GetStatus();
    }
}
