using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.OrleansInterfaces.Structures
{
    [Serializable]
    public struct Position2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position2(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }

        public Position2 Offset(int dX, int dY)
        {
            return new Position2(X + dX, Y + dY);
        }
    }
}
