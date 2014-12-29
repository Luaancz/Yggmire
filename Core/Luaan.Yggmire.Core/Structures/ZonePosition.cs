using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.Core.Structures
{
    [Serializable]
    public struct ZonePosition
    {
        public int Layer;
        public Position2 Position;

        public ZonePosition(int layer, Position2 position)
            : this()
        {
            Layer = layer;
            Position = position;
        }

        public ZonePosition Offset(int dX, int dY)
        {
            return new ZonePosition(Layer, Position.Offset(dX, dY));
        }
    }
}
