using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luaan.Yggmire.OrleansInterfaces.Structures;

namespace Luaan.Yggmire.OrleansInterfaces.Actors
{
    [Serializable]
    public class ZoneItem
    {
        public int Id { get; set; }
        public int PrototypeId { get; set; }

        public Position2 Position { get; set; }
    }
}
