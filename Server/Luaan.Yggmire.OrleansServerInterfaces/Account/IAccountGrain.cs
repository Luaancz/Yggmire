using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Orleans;

using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Account;

namespace Luaan.Yggmire.OrleansServerInterfaces.Account
{
    /// <summary>
    /// Orleans grain communication interface IAccountGrain
    /// </summary>
    [ExtendedPrimaryKey]
    public interface IAccountGrain : Orleans.IGrain
    {
        Task<string> Name { get; }

        Task CaptureSession(ISessionGrain session);
        Task<AccountInformation> GetState();

        Task Create(string password);

        Task<bool> ValidatePassword(string password);
    }
}
