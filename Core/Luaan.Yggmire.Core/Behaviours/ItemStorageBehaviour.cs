﻿using Luaan.Yggmire.Core.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.Core.Behaviours
{
    [Serializable]
    public class ItemStorageBehaviour : IItemBehaviour<ItemStorageBehaviourState>
    {
        /// <summary>
        /// Id used to pair the configured prototypes of the item with the actual stored state for that behaviour.
        /// </summary>
        public string Id { get; set; }

        public bool Public { get; set; }
    }

    [Serializable]
    public class ItemStorageBehaviourState : IItemBehaviourState
    {
        public string BehaviourId { get; set; }
        public List<InventoryItem> Items { get; set; }
    }
}
