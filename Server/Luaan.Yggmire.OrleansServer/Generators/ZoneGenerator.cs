using Luaan.Yggmire.Core.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var item = new StaticWorldItem { Id = i, Position = new Position2(rnd.Next(200000) - 100000, rnd.Next(200000) - 100000), PrototypeId = 1 };
                    gen.WorldItems.Add(item);
                }

                if (position.Position.X == 0 && position.Position.Y == 0)
                {
                    gen.WorldItems.AddRange(GetWorldItems());
                }
            }
            
            return gen;
        }


        static WorldItem[] GetWorldItems()
        {
            return new WorldItem[] 
                { 
                    new StaticWorldItem { Id = 101, Position = new Position2(-100000, -100000), PrototypeId = 1},
                    new StaticWorldItem { Id = 102, Position = new Position2( 100000, -100000), PrototypeId = 1},
                    new StaticWorldItem { Id = 103, Position = new Position2( 100000,  100000), PrototypeId = 2},
                    new StaticWorldItem { Id = 104, Position = new Position2(-100000,  100000), PrototypeId = 1},
                };
        }
    }
}
