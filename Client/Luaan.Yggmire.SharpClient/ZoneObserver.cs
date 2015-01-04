using Luaan.Yggmire.Core.Structures;
using Luaan.Yggmire.OrleansInterfaces.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.SharpClient
{
    public class ZoneObserver : IZoneObserver
    {
        private readonly YggmireGame game;

        public ZoneObserver(YggmireGame game)
        {
            this.game = game;
        }

        public void AddZone(ZonePosition zonePosition)
        {
            game.AddZone(zonePosition);
        }

        public void AddItems(ZonePosition zonePosition, WorldItem[] items)
        {
            foreach (var item in items)
                game.AddWorldItem(zonePosition, item);
        }

        public void DropZone(ZonePosition zonePosition)
        {
            game.DropZone(zonePosition);
        }
    }
}
