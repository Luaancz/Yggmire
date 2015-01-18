using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luaan.Yggmire.OrleansInterfaces.Structures;
using Luaan.Yggmire.OrleansInterfaces.Actors;
using Luaan.Yggmire.OrleansServer.Behaviours;

namespace Luaan.Yggmire.OrleansServer.Actors
{
    [Serializable]
    public class WorldItem
    {
        public WorldItem()
        {
            Behaviours = new List<IItemBehaviour>();
            BehaviourStates = new Dictionary<string, IItemBehaviourState>();
        }

        public int Id { get; set; }
        public int PrototypeId { get; set; }

        public Position2 Position { get; set; }

        /// <summary>
        /// This is loaded from the world item prototype.
        /// </summary>
        public List<IItemBehaviour> Behaviours { get; private set; }

        // HACK: The ID has to be a string because of the JSON serializer for now.
        public Dictionary<string, IItemBehaviourState> BehaviourStates { get; set; }

        public ZoneItem ToClient()
        {
            return new ZoneItem { Id = Id, PrototypeId = PrototypeId, Position = Position };
        }
    }
}
