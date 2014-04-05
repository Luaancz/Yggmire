using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Luaan.Yggmire.OrleansInterfaces.Account;

namespace Luaan.Yggmire.OrleansInterfaces
{
    /// <summary>
    /// Orleans grain communication interface ISessionGrain
    /// </summary>
    public interface ISessionGrain : Orleans.IGrain
    {
        Task<AccountInformation> Authorize(string name, string password);
        Task<AccountInformation> GetAccount();

        Task<AccountInformation> CreateAccount(string name, string password);
    }
}
