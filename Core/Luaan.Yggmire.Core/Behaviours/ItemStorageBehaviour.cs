using Luaan.Yggmire.Core.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.Core.Behaviours
{
    [Serializable]
    public class ItemStorageBehaviour : IItemBehaviour<ItemStorageBehaviourState>, IItemBehaviourWithActions
    {
        /// <summary>
        /// Id used to pair the configured prototypes of the item with the actual stored state for that behaviour.
        /// </summary>
        public string Id { get; set; }

        public bool IsContainer { get; set; }

        public ItemAction[] GetAvailableActions(WorldItem parent)
        {
            if (IsContainer)
            {
                return new ItemAction[] { new ItemAction { Id = "open", Name = "Open" } };
            }

            return new ItemAction[] { };
        }

        public void ExecuteAction(WorldItem parentItem, /* ISessionGrain session, */string actionId)
        {
            if (IsContainer)
            {
                switch (actionId)
                {
                    case "open":
                        {
                            // TODO: Hmm... what should this do and how? :D

                            break;
                        }
                }
            }
        }
    }

    [Serializable]
    public class ItemStorageBehaviourState : IItemBehaviourState
    {
        public string BehaviourId { get; set; }
        public List<InventoryItem> Items { get; set; }
    }
}
