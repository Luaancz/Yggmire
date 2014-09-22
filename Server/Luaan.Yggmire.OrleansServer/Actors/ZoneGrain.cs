using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Orleans;
using Luaan.Yggmire.OrleansServerInterfaces.Actors;
using Luaan.Yggmire.OrleansInterfaces.Actors;
using Luaan.Yggmire.Core.Structures;

namespace Luaan.Yggmire.OrleansServer.Actors
{
    /// <summary>
    /// Orleans grain implementation class ZoneGrain
    /// </summary>
    public class ZoneGrain : Grain<IZoneState>, IZoneGrain
    {
        private ObserverSubscriptionManager<IZoneObserver> subscribers;

        public override Task ActivateAsync()
        {
            subscribers = new ObserverSubscriptionManager<IZoneObserver>();

            return base.ActivateAsync();
        }

        public override Task DeactivateAsync()
        {
            subscribers.Clear();

            return base.DeactivateAsync();
        }

        WorldItem[] GetWorldItems()
        {
            return new WorldItem[] 
                { 
                    new StaticWorldItem { Id = 1, Position = new Position2(-100000, -100000), PrototypeId = 1},
                    new StaticWorldItem { Id = 2, Position = new Position2( 100000, -100000), PrototypeId = 1},
                    new StaticWorldItem { Id = 3, Position = new Position2( 100000,  100000), PrototypeId = 2},
                    new StaticWorldItem { Id = 4, Position = new Position2(-100000,  100000), PrototypeId = 1},
                };
        }

        Task IZoneGrain.Subscribe(IZoneObserver observer)
        {
            subscribers.Subscribe(observer);

            observer.AddItems(this.GetPrimaryKey(), GetWorldItems());

            return TaskDone.Done;
        }

        Task IZoneGrain.Unsubscribe(IZoneObserver observer)
        {
            subscribers.Unsubscribe(observer);

            return TaskDone.Done;
        }
    }

    public interface IZoneState : IGrainState
    {

    }
}
