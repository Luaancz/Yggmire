using Luaan.Yggmire.OrleansInterfaces.Actors;
using Luaan.Yggmire.OrleansServerInterfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.OrleansServer.Actors
{
    public class ZoneManager
    {
        IZoneObserver zoneObserver;

        IZoneGrain currentZone;

        List<IZoneGrain> neighbours;

        public ZoneManager(IZoneObserver zoneObserver)
        {
            this.zoneObserver = zoneObserver;
            this.neighbours = new List<IZoneGrain>();
        }

        public async Task EnterZone(IZoneGrain zone)
        {
            // At this point we'll only handle one zone, without the neighbours.
            var toUnsubscribe = new List<IZoneGrain>();
            var toSubscribe = new List<IZoneGrain>();

            var newNeighbours = (await zone.GetNeighbours()).ToList();

            if (currentZone != null)
            {
                toUnsubscribe.Add(currentZone);
                toUnsubscribe.AddRange(neighbours.Where(i => !newNeighbours.Contains(i)));

                await Task.WhenAll(toUnsubscribe.Select(i => i.Unsubscribe(zoneObserver)));
            }

            toSubscribe.Add(zone);
            toSubscribe.AddRange(newNeighbours.Where(i => !neighbours.Contains(i)));
            
            currentZone = zone;
            neighbours = newNeighbours;

            await Task.WhenAll(toSubscribe.Select(i => i.Subscribe(zoneObserver)));
        }
    }
}
