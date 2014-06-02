using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Luaan.Yggmire.OrleansServerInterfaces
{
    /// <summary>
    /// Orleans grain communication interface IClientConversationGrain
    /// </summary>
    public interface IClientConversationGrain : Orleans.IGrain
    {
        Task<string> AskForText(string title, string question);
    }
}
