using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Orleans;

using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansServerInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Account;
using Luaan.Yggmire.OrleansServerInterfaces.Account;

namespace Luaan.Yggmire.OrleansServer.Account
{
    /// <summary>
    /// Orleans grain implementation class AccountGrain
    /// </summary>
    [StorageProvider(ProviderName = "Default")]
    public class AccountGrain : GrainBase<IAccountGrainState>, IAccountGrain
    {
        ISessionGrain currentSession;

        public override Task ActivateAsync()
        {
            if (State.Id == Guid.Empty)
            {
                State.Id = Guid.NewGuid();
                State.Characters = new List<CharacterInformation>();
            }

            return base.ActivateAsync();
        }

        Task<string> IAccountGrain.Name
        {
            get
            {
                return Task.FromResult(State.Name);
            }
        }
        
        Task IAccountGrain.CaptureSession(ISessionGrain session)
        {
            if (currentSession != null && session != null)
                throw new InvalidOperationException("The account is already logged in.");

            currentSession = session;

            State.LastLogin = DateTimeOffset.Now;
            return State.WriteStateAsync();
        }

        Task IAccountGrain.Create(string password)
        {
            if (!string.IsNullOrEmpty(State.Name))
                throw new InvalidOperationException("The specified account already exists.");

            if (string.IsNullOrEmpty(password) || password.Length < 6)
                throw new ArgumentException("The password has to be at least six characters long.", "password");

            string name;
            this.GetPrimaryKey(out name);

            State.Name = name;
            State.Password = password;
            State.CreatedOn = DateTimeOffset.Now;

            return State.WriteStateAsync();
        }

        async Task<ICharacterGrain> IAccountGrain.CreateCharacter()
        {
            var grain = CharacterGrainFactory.GetGrain(Guid.NewGuid());
            
            await grain.BindAccount(this);

            return grain;
        }

        async Task<ICharacterGrain> IAccountGrain.SelectCharacter(string name)
        {
            var character = State.Characters.FirstOrDefault(i => i.Name == name);

            if (character == null)
                throw new InvalidOperationException("No character with such name.");

            var grain = CharacterGrainFactory.GetGrain(character.Id);

            return grain;
        }

        Task IAccountGrain.CompleteCharacter(ICharacterGrain grain, CharacterInformation info)
        {
            State.Characters.Add(info);

            return State.WriteStateAsync();
        }

        Task<AccountInformation> IAccountGrain.GetState()
        {
            return Task.FromResult(new AccountInformation { Name = State.Name });
        }

        Task<bool> IAccountGrain.ValidatePassword(string password)
        {
            return Task.FromResult(State.Password == password);
        }
    }
    
    public interface IAccountGrainState : IGrainState
    {
        Guid Id { get; set; }

        string Name { get; set; }
        // Secret! :) Obviously, we will not be storing the password in plain text in production.
        string Password { get; set; }

        string Email { get; set; }

        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset LastLogin { get; set; }

        List<CharacterInformation> Characters { get; set; }
    }
}
