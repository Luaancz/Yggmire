using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Chat;
using Luaan.Yggmire.OrleansServerInterfaces.Chat;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.OrleansServer.Chat
{    
    public class ChatGrain : GrainBase, IChatGrain
    {
        private ObserverSubscriptionManager<IChatObserver> subscribers;

        public override Task ActivateAsync()
        {
            subscribers = new ObserverSubscriptionManager<IChatObserver>();

            return base.ActivateAsync();
        }

        public override Task DeactivateAsync()
        {
            subscribers.Clear();

            return base.DeactivateAsync();
        }

        Task IChatGrain.SendMessage(string name, string message)
        {
            subscribers.Notify((o) => o.UpdateChat(name, message));

            return TaskDone.Done;
        }

        Task IChatGrain.Subscribe(string name, IChatObserver observer)
        {
            subscribers.Notify((o) => o.UpdateChat(null, name + " has joined the channel."));
            subscribers.Subscribe(observer);

            return TaskDone.Done;
        }

        Task IChatGrain.Unsubscribe(string name, IChatObserver observer)
        {
            subscribers.Unsubscribe(observer);
            subscribers.Notify((o) => o.UpdateChat(null, name + " has left the channel."));

            return TaskDone.Done;
        }
    }
}
