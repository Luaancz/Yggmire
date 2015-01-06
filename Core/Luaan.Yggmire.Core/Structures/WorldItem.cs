using Luaan.Yggmire.Core.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.Core.Structures
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

        /// <summary>
        /// This is loaded from the world item prototype.
        /// </summary>
        // TODO: Shouldn't be serialized
        public List<IItemBehaviour> Behaviours { get; set; }

        // HACK: The ID has to be a string because of the JSON serializer for now.
        public Dictionary<string, IItemBehaviourState> BehaviourStates { get; set; }
    }
}
