using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansServerInterfaces;
using Orleans;
using Luaan.Yggmire.OrleansServerInterfaces.Account;
using Luaan.Yggmire.OrleansInterfaces.Account;

namespace Luaan.Yggmire.OrleansServer.Account
{
    [StorageProvider(ProviderName = "Default")]
    public class CharacterGrain : GrainBase<ICharacterGrainState>, ICharacterGrain
    {
        IAccountGrain account;
        bool isComplete;

        public override Task ActivateAsync()
        {
            isComplete = !string.IsNullOrEmpty(State.Name);

            return base.ActivateAsync();
        }

        Task<bool> ICharacterGrain.IsComplete
        {
            get { return Task.FromResult(isComplete); }
        }

        Task ICharacterGrain.BindAccount(IAccountGrain account)
        {
            this.account = account;

            return TaskDone.Done;
        }

        Task<string> ICharacterGrain.Name { get { return Task.FromResult(State.Name); } }

        Task ICharacterGrain.SetName(string name)
        {
            if (isComplete)
                throw new InvalidOperationException("Cannot change the name of an existing character.");

            State.Name = name;
            return TaskDone.Done;
        }

        Task ICharacterGrain.Complete()
        {
            isComplete = true;

            string accountName;

            account.GetPrimaryKey(out accountName);

            State.AccountId = accountName;

            return State.WriteStateAsync();
        }

        Task<CharacterInformation> ICharacterGrain.GetInfo()
        {
            return Task.FromResult(new CharacterInformation { Id = this.GetPrimaryKey(), Name = State.Name });
        }
    }
}
