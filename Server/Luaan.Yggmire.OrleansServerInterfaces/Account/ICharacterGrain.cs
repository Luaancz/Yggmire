using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Luaan.Yggmire.OrleansInterfaces.Account;

namespace Luaan.Yggmire.OrleansServerInterfaces.Account
{
    public interface ICharacterGrain : IGrain
    {
        Task<bool> IsComplete();

        Task BindAccount(IAccountGrain account);

        Task<string> GetName();
        Task SetName(string name);

        Task Complete();

        Task<CharacterInformation> GetInfo();

        Task<Guid> GetZoneId();
    }

    public interface ICharacterGrainState : IGrainState
    {
        string AccountId { get; set; }
        string Name { get; set; }

        Guid ZoneId { get; set; }
    }
}
