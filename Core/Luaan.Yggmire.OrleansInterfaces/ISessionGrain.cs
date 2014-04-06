using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Luaan.Yggmire.OrleansInterfaces.Account;
using Luaan.Yggmire.OrleansInterfaces.Chat;

namespace Luaan.Yggmire.OrleansInterfaces
{
    /// <summary>
    /// Orleans grain communication interface ISessionGrain
    /// </summary>
    public interface ISessionGrain : Orleans.IGrain
    {
        Task<string> PlayerName { get; }

        Task<AccountInformation> Authorize(string name, string password);
        Task<AccountInformation> GetAccount();

        Task<AccountInformation> CreateAccount(string name, string password);

        Task SubscribeForChat(int channel, IChatObserver observer);
        Task UnsubscribeFromChat(int channel, IChatObserver observer);
        Task SendChatMessage(int channel, string message);
    }
}
