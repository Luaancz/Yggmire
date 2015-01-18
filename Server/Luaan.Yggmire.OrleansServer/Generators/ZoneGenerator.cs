using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luaan.Yggmire.OrleansInterfaces.Structures;
using Luaan.Yggmire.OrleansServer.Actors;
using Luaan.Yggmire.OrleansServer.Behaviours;

namespace Luaan.Yggmire.OrleansServer.Generators
{
    public class ZoneGenerator
    {
        private ZonePosition position;

        private ZoneGenerator(ZonePosition position)
        {
            this.position = position;
        }

        public List<WorldItem> WorldItems { get; set; }

        public static ZoneGenerator Generate(ZonePosition position)
        {
            var gen = new ZoneGenerator(position);

            gen.WorldItems = new List<WorldItem>();

            if (position.Layer == 50)
            {
                var rnd = new Random(position.Layer * 1000 + position.Position.X ^ 73 + position.Position.Y * 95633);

                var treeCount = rnd.Next(10);

                for (var i = 0; i < treeCount; i++)
                {
                    var item = WorldItemCreator.CreateWorldItem(i, 1);
                    item.Position = new Position2(rnd.Next(200000) - 100000, rnd.Next(200000) - 100000);
                    item.BehaviourStates.Add("1", new ItemStorageBehaviourState { BehaviourId = "1", Items = new List<InventoryItem> { new InventoryItem { PrototypeId = 1, Name = "Branch", Amount = 5 } } });

                    gen.WorldItems.Add(item);
                }

                if (position.Position.X == 0 && position.Position.Y == 0)
                {
                    var box = WorldItemCreator.CreateWorldItem(103, 2);
                    box.Position = new Position2(100000,  100000);

                    box.BehaviourStates.Add("contents", new ItemStorageBehaviourState { BehaviourId = "contents", Items = new List<InventoryItem> { new InventoryItem { PrototypeId = 1, Name = "Branch", Amount = 25 } } });

                    gen.WorldItems.Add(box);
                }
            }
            
            return gen;
        }
    }
}
