using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luaan.Yggmire.Core;
using Luaan.Yggmire.Core.Structures;

namespace Luaan.Yggmire.OrleansInterfaces.Actors
{
    public interface IZoneObserver : IGrainObserver
    {
        void AddZone(ZonePosition zonePosition);
        void AddItems(ZonePosition zonePosition, WorldItem[] items);
        void DropZone(ZonePosition zonePosition);
    }
}
