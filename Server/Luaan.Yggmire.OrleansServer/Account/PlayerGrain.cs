using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansServerInterfaces;
using Orleans;
using Luaan.Yggmire.OrleansServerInterfaces.Account;

namespace Luaan.Yggmire.OrleansServer.Account
{
    public class PlayerGrain : GrainBase<IPlayerGrainState>, IPlayerGrain
    {

    }

    public interface IPlayerGrainState : IGrainState
    {
        string Name { get; set; }
    }
}
