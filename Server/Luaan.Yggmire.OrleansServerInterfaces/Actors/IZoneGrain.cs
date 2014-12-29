using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using Luaan.Yggmire.OrleansInterfaces.Actors;

namespace Luaan.Yggmire.OrleansServerInterfaces.Actors
{
    /// <summary>
    /// Orleans grain communication interface IZoneGrain
    /// </summary>
    public interface IZoneGrain : Orleans.IGrainWithStringKey
    {
        Task<IEnumerable<IZoneGrain>> GetNeighbours();

        Task Subscribe(IZoneObserver observer);
        Task Unsubscribe(IZoneObserver observer);
    }
}
