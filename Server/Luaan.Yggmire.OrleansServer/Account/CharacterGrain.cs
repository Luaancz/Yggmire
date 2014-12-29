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
using Orleans.Providers;

namespace Luaan.Yggmire.OrleansServer.Account
{
    [StorageProvider(ProviderName = "Default")]
    public class CharacterGrain : Grain<ICharacterGrainState>, ICharacterGrain
    {
        IAccountGrain account;
        bool isComplete;

        public override Task ActivateAsync()
        {
            isComplete = !string.IsNullOrEmpty(State.Name);

            return base.ActivateAsync();
        }

        public async override Task DeactivateAsync()
        {
            if (!isComplete)
            {
                await State.ClearStateAsync();
            }

            await base.DeactivateAsync();
        }

        Task<bool> ICharacterGrain.IsComplete()
        {
            return Task.FromResult(isComplete);
        }

        Task ICharacterGrain.BindAccount(IAccountGrain account)
        {
            this.account = account;

            return TaskDone.Done;
        }

        Task<string> ICharacterGrain.GetName() 
        { 
            return Task.FromResult(State.Name);
        }

        async Task ICharacterGrain.SetName(string name)
        {
            if (isComplete)
                throw new InvalidOperationException("Cannot change the name of an existing character.");

            var accountState = await account.GetState();

            if (accountState.Characters.Any(i => StringComparer.InvariantCultureIgnoreCase.Compare(name, i.Name) == 0))
                throw new InvalidOperationException("A character with the same name already exists on your account.");

            State.Name = name;
        }

        Task ICharacterGrain.Complete()
        {
            isComplete = true;

            string accountName;

            account.GetPrimaryKey(out accountName);

            State.AccountId = accountName;
            // TODO: Pick a random starting zone
            State.ZoneId = "50.0.0";

            return Task.WhenAll(State.WriteStateAsync(), account.CompleteCharacter(this, new CharacterInformation { Id = this.GetPrimaryKey(), Name = State.Name }));
        }

        Task<CharacterInformation> ICharacterGrain.GetInfo()
        {
            return Task.FromResult(new CharacterInformation { Id = this.GetPrimaryKey(), Name = State.Name });
        }

        Task<string> ICharacterGrain.GetZoneId()
        {
            return Task.FromResult(State.ZoneId);
        }
    }
}
