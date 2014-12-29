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
        void AddItems(string zoneId, WorldItem[] items);
    }
}
