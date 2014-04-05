using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Orleans;
using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansServerInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Account;

namespace Luaan.Yggmire.OrleansServer
{
    /// <summary>
    /// Orleans grain implementation class SessionGrain
    /// </summary>
    public class SessionGrain : Orleans.GrainBase, ISessionGrain
    {
        IAccountGrain loggedAccount;

        /// <summary>
        /// When the session dies, we have to clean up a bit.
        /// </summary>
        /// <returns></returns>
        public override Task DeactivateAsync()
        {
            if (loggedAccount != null)
                loggedAccount.CaptureSession(null);

            return base.DeactivateAsync();
        }

        async Task<AccountInformation> CompleteLogin(IAccountGrain account)
        {
            await account.CaptureSession(this);
            loggedAccount = account;

            return await account.GetState();
        }

        /// <summary>
        /// Checks the account credentials, and if they are valid, completes the login process.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        async Task<AccountInformation> ISessionGrain.Authorize(string name, string password)
        {
            var account = AccountGrainFactory.GetGrain(0, name);

            if (account != null && await account.ValidatePassword(password))
            {
                return await CompleteLogin(account);
            }
            else
                throw new UnauthorizedAccessException("Wrong username or password.");
        }

        /// <summary>
        /// Creates a new account with the specified name and password, and if everything goes well, completes the login process.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        async Task<AccountInformation> ISessionGrain.CreateAccount(string name, string password)
        {
            var account = AccountGrainFactory.GetGrain(0, name);
            
            await account.Create(password);

            return await CompleteLogin(account);
        }

        Task<AccountInformation> ISessionGrain.GetAccount()
        {
            if (loggedAccount == null)
                throw new UnauthorizedAccessException("Not logged in or session expired.");

            return loggedAccount.GetState();
        }
    }
}
