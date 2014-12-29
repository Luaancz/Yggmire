using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Orleans;
using Luaan.Yggmire.OrleansServerInterfaces.Actors;
using Luaan.Yggmire.OrleansInterfaces.Actors;
using Luaan.Yggmire.Core.Structures;
using Orleans.Providers;

namespace Luaan.Yggmire.OrleansServer.Actors
{
    /// <summary>
    /// Orleans grain implementation class ZoneGrain
    /// </summary>
    [StorageProvider(ProviderName = "Default")]
    public class ZoneGrain : Grain<IZoneState>, IZoneGrain
    {
        private ObserverSubscriptionManager<IZoneObserver> subscribers;
        private List<IZoneGrain> neighbours;

        IZoneGrain GetZoneByPosition(ZonePosition position)
        {
            return ZoneGrainFactory.GetGrain(string.Format("{0}.{1}.{2}", position.Layer, position.Position.X, position.Position.Y));
        }

        public override async Task ActivateAsync()
        {
            subscribers = new ObserverSubscriptionManager<IZoneObserver>();

            if (State.Id == null)
            {
                string strid;
                this.GetPrimaryKey(out strid);

                State.Id = strid;

                var location = strid.Split('.').Select(i => int.Parse(i)).ToArray();
                State.Position = new ZonePosition(location[0], new Position2(location[1], location[2]));
                State.Items = new List<WorldItem> { };

                if (strid == "50.0.0") State.Items.AddRange(GetWorldItems());

                await State.WriteStateAsync();
            }

            neighbours = new List<IZoneGrain>();

            if (State.Position.Layer > 0 && State.Position.Layer < 100)
            {
                neighbours.AddRange
                    (
                        new[]
                        {
                            GetZoneByPosition(State.Position.Offset(-1, -1)),
                            GetZoneByPosition(State.Position.Offset(-1,  0)),
                            GetZoneByPosition(State.Position.Offset(-1,  1)),
                            GetZoneByPosition(State.Position.Offset( 0, -1)),
                            GetZoneByPosition(State.Position.Offset( 0,  1)),
                            GetZoneByPosition(State.Position.Offset( 1, -1)),
                            GetZoneByPosition(State.Position.Offset( 1,  0)),
                            GetZoneByPosition(State.Position.Offset( 1,  1)),
                        }
                    );
            }

            await base.ActivateAsync();
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

        Task<IEnumerable<IZoneGrain>> IZoneGrain.GetNeighbours()
        {
            return Task.FromResult(neighbours.AsEnumerable());
        }

        Task IZoneGrain.Subscribe(IZoneObserver observer)
        {
            subscribers.Subscribe(observer);

            observer.AddItems(State.Id, State.Items.ToArray());

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
        string Id { get; set; }

        List<WorldItem> Items { get; set; }
        ZonePosition Position { get; set; }
    }
}
