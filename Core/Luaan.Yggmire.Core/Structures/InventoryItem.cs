using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.Core.Structures
{
    [Serializable]
    public class InventoryItem
    {
        public int PrototypeId { get; set; }

        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
