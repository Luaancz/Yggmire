using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luaan.Yggmire.OrleansServer.Actors;
using Luaan.Yggmire.OrleansInterfaces.Structures;

namespace Luaan.Yggmire.OrleansServer.Behaviours
{
    public interface IItemBehaviourWithActions : IItemBehaviour
    {
        ItemAction[] GetAvailableActions(WorldItem parent);
        void ExecuteAction(string actionId);
    }
}
