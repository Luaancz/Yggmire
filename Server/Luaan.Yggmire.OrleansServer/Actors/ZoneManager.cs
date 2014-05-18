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

        public ZoneManager(IZoneObserver zoneObserver)
        {
            this.zoneObserver = zoneObserver;
        }

        public async Task EnterZone(IZoneGrain zone)
        {
            // TODO: We have to leave and enter zones around this one.

            // At this point we'll only handle one zone, without the neighbours.
            if (currentZone != null)
                await currentZone.Unsubscribe(zoneObserver);

            currentZone = zone;

            await zone.Subscribe(zoneObserver);
        }
    }
}
