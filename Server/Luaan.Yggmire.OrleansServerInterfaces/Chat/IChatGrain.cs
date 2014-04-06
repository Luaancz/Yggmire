using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Chat;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.OrleansServerInterfaces.Chat
{
    public interface IChatGrain : IGrain
    {
        Task SendMessage(string name, string message);

        Task Subscribe(string name, IChatObserver observer);
        Task Unsubscribe(string name, IChatObserver observer);
    }
}
