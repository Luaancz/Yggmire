using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.OrleansInterfaces.Chat
{
    public interface IChatObserver : IGrainObserver
    {
        void UpdateChat(string name, string message);
    }
}
