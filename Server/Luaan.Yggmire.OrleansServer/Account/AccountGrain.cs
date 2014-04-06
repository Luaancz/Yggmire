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
        string Name { get; set; }
        // Secret! :) Obviously, we will not be storing the password in plain text in production.
        string Password { get; set; }

        string Email { get; set; }

        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset LastLogin { get; set; }

        List<Guid> Players { get; set; }
    }
}
